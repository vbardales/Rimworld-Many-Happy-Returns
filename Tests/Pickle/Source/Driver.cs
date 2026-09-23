using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// Shared lookups for every step class here. Nothing is cached: a scenario tagged
    /// <c>@same-world</c> and a save reload both replace every object in the game, and a helper
    /// holding yesterday's pawn would assert against an object nothing draws from any more.
    /// Every unguarded hop through reflection reports the same thing when it misses - "Object
    /// reference not set to an instance of an object" - and the report keeps no stack, so these
    /// name themselves instead. Field/Method/Property mirror the convention already used across
    /// this collection (see SkillIcons/Tests/Pickle/Source/Driver.cs).
    /// </summary>
    public static class Driver
    {
        internal const BindingFlags StaticAny = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        internal const BindingFlags InstanceAny = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static FieldInfo Field(PickleContext ctx, Type owner, string name, BindingFlags flags)
        {
            var field = owner.GetField(name, flags);
            ctx.Require(field != null,
                $"{owner.FullName}.{name} no longer exists: the mod or the game renamed it, update the steps");
            return field;
        }

        public static MethodInfo Method(PickleContext ctx, Type owner, string name, BindingFlags flags)
        {
            var method = owner.GetMethod(name, flags);
            ctx.Require(method != null,
                $"{owner.FullName}.{name}() no longer exists: the mod or the game renamed it, update the steps");
            return method;
        }

        public static PropertyInfo Property(PickleContext ctx, Type owner, string name, BindingFlags flags)
        {
            var property = owner.GetProperty(name, flags);
            ctx.Require(property != null,
                $"{owner.FullName}.{name} no longer exists: the mod or the game renamed it, update the steps");
            return property;
        }

        public static ManyHappyReturnsMod Mod(PickleContext ctx)
        {
            var mod = LoadedModManager.GetMod<ManyHappyReturnsMod>();
            ctx.Require(mod != null,
                "LoadedModManager.GetMod<ManyHappyReturnsMod>() returned nothing: ManyHappyReturns.dll "
                + "is not loaded in this session, so no step here can reach its settings or its state");
            return mod;
        }

        /// <summary>Read through the mod's own static property, never cached: a stale reference
        /// would be a different object after SettingsSandbox resets it or a save reloads.</summary>
        public static ManyHappyReturnsSettings Settings(PickleContext ctx)
        {
            Mod(ctx);
            ctx.Require(ManyHappyReturnsMod.Settings != null,
                "ManyHappyReturnsMod.Settings is null: the mod constructor did not run, which means "
                + "the assembly loaded but its Mod class did not");
            return ManyHappyReturnsMod.Settings;
        }

        /// <summary>The per-game component. Follows the Game object: null at the main menu, a
        /// different instance after a reload. Never held on to across a step boundary.</summary>
        public static GameComponent_Birthdays Component(PickleContext ctx)
        {
            var component = Current.Game?.GetComponent<GameComponent_Birthdays>();
            ctx.Require(component != null,
                "no GameComponent_Birthdays on the current game: this step needs "
                + "'the save \"...\" is loaded' first, or the mod's assembly did not register it");
            return component;
        }

        public static Map Map(PickleContext ctx)
        {
            var map = Find.CurrentMap;
            ctx.Require(map != null, "no current map: load a fixture with a colony before this step");
            return map;
        }

        /// <summary>
        /// The awake, present, humanlike free colonists of the current map, in the game's own
        /// enumeration order (stable within a scenario, not across a reload). TEST_SCENARIOS.md's
        /// common setup asks for at least three; every scenario that needs a celebrant, a wisher or
        /// a witness reads from this list rather than a fixed name the fixture might not carry.
        /// </summary>
        public static List<Pawn> Colonists(PickleContext ctx, int atLeast = 1)
        {
            List<Pawn> colonists = Map(ctx).mapPawns.FreeColonistsSpawned
                .Where(BirthdayUtility.CanCelebrate)
                .Where(p => !p.Downed && p.Awake())
                .ToList();

            ctx.Require(colonists.Count >= atLeast,
                $"the map has {colonists.Count} awake, eligible free colonists; this scenario needs "
                + $"at least {atLeast}. TEST_SCENARIOS.md's common setup asks for three");
            return colonists;
        }

        /// <summary>
        /// Moves a pawn's birth date onto today, at the same whole-year chronological age, exactly as
        /// the mod's own "Birthday is today" debug action does. Reflects into that private method
        /// instead of reimplementing its math, so a scenario here exercises the very code path a
        /// developer uses by hand, and never drifts from it if that math ever changes.
        /// </summary>
        public static void MoveBirthdayToToday(PickleContext ctx, Pawn pawn)
        {
            MethodInfo method = Method(ctx, typeof(DebugActions_Birthday), "MakeBirthdayToday",
                BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { pawn });
        }

        /// <summary>
        /// Forces the stored value of a record, bypassing the game's own accumulation. Used to give a
        /// fixture pawn - almost always long-established - a short tenure, so the "recruit from
        /// yesterday" exemption (TEST_SCENARIOS.md S10) can be exercised without waiting real days.
        /// DefMap's own indexer is public; only the private field holding the DefMap is reflected.
        /// </summary>
        public static void SetRecordValue(PickleContext ctx, Pawn pawn, RecordDef def, float value)
        {
            FieldInfo field = Field(ctx, typeof(Pawn_RecordsTracker), "records", InstanceAny);
            var records = (DefMap<RecordDef, float>)field.GetValue(pawn.records);
            records[def] = value;
        }

        /// <summary>All memories a pawn formed within today's window, read live every call.</summary>
        public static List<Thought_Memory> TodaysMemories(Pawn pawn)
        {
            List<Thought_Memory> memories = pawn.needs?.mood?.thoughts?.memories?.Memories;
            return memories == null
                ? new List<Thought_Memory>()
                : memories.Where(m => m.age <= BirthdayUtility.TodayWindowTicks).ToList();
        }

        public static bool HasMemoryOfDef(Pawn pawn, ThoughtDef def) =>
            TodaysMemories(pawn).Any(m => m.def == def);

        public static Thought_Memory MemoryOfDef(PickleContext ctx, Pawn pawn, ThoughtDef def)
        {
            Thought_Memory memory = TodaysMemories(pawn).FirstOrDefault(m => m.def == def);
            ctx.Assert(memory != null,
                $"{pawn.LabelShortCap} holds no memory of {def.defName} formed today. Today's memories: "
                + string.Join(", ", TodaysMemories(pawn).Select(m => m.def.defName).ToArray()));
            return memory;
        }

        /// <summary>
        /// Places <paramref name="mover"/> within the six-cell, line-of-sight range
        /// SocialInteractionUtility.IsGoodPositionForInteraction requires, so a wish exchange does
        /// not depend on where the fixture happened to leave two colonists standing. Thing.Position's
        /// own public setter is used, not a raw field write: it runs the game's own region/listener
        /// bookkeeping for a spawned pawn.
        /// </summary>
        public static void BringTogether(PickleContext ctx, Pawn mover, Pawn target)
        {
            Map map = Map(ctx);
            bool found = CellFinder.TryRandomClosewalkCellNear(target.Position, map, 4, out IntVec3 cell,
                c => SocialInteractionUtility.IsGoodPositionForInteraction(c, target.Position, map));
            ctx.Require(found,
                $"no walkable cell in line of sight of {target.LabelShortCap} was found within 4 "
                + "tiles: the fixture's map layout does not leave room for this scenario");
            mover.Position = cell;
        }

        // --- Scenario roles -------------------------------------------------------------------
        //
        // Scenarios refer to "the celebrant" and "the wisher" rather than to a fixed pawn name,
        // because the fixture's exact colonist roster is not something these steps control. A
        // role is picked once per scenario by a Given step in RoleSteps.cs.
        //
        // What is remembered is the pawn's persistent Name, not the C# Pawn reference: "I save and
        // reload" replaces every object in the game, and a role holding the old reference would
        // assert against a Pawn nothing draws from any more (the authoring guide: "reacquire pawns
        // after a load"). Resolving by name on every call costs a map scan, but makes every
        // scenario - reload or not - immune to that trap without a separate reacquisition step.
        // PickleContext.Set/Get key by TYPE, one slot each: two roles need two wrapper types.

        public sealed class CelebrantRole
        {
            public string NameKey;
        }

        public sealed class WisherRole
        {
            public string NameKey;
        }

        public static string NameKeyOf(Pawn pawn) => pawn.Name?.ToStringFull ?? pawn.LabelShortCap;

        private static Pawn ResolveRole<TRole>(PickleContext ctx, string missingMessage, Func<TRole, string> keyOf)
        {
            TRole role;
            try
            {
                role = ctx.Get<TRole>();
            }
            catch (InvalidOperationException)
            {
                ctx.Require(false, missingMessage);
                return null;
            }

            string key = keyOf(role);
            Pawn found = Colonists(ctx).FirstOrDefault(p => NameKeyOf(p) == key);
            ctx.Require(found != null,
                $"no colonist named '{key}' is on the map any more: this scenario's celebrant or "
                + "wisher left, died, or the save reloaded onto a different fixture");
            return found;
        }

        public static Pawn Celebrant(PickleContext ctx) => ResolveRole<CelebrantRole>(ctx,
            "no celebrant has been picked yet: this scenario needs "
            + "'the celebrant is the map's first eligible colonist' first",
            role => role.NameKey);

        public static Pawn Wisher(PickleContext ctx) => ResolveRole<WisherRole>(ctx,
            "no wisher has been picked yet: this scenario needs "
            + "'the wisher is the map's second eligible colonist' first",
            role => role.NameKey);
    }
}
