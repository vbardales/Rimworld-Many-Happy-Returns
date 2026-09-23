using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// The exemptions of TEST_SCENARIOS.md S10 that need a pawn put in a state the fixture does not
    /// hold: Downed, away in a caravan, too few witnesses, and a newborn. Every scenario begins by
    /// loading the saved fixture again, so nothing done to a pawn here outlives its scenario and no
    /// teardown is needed; this is what the README's earlier claim of a "shared fixture" risk got wrong.
    ///
    /// Where the game already has the API a developer uses for the same thing, it is that API:
    /// HealthUtility.DamageUntilDowned, and CaravanExitMapUtility.ExitMapAndCreateCaravan, which is
    /// what the game's own "form caravan" flow ends in.
    /// </summary>
    [PickleSteps]
    public class ExemptionSteps
    {
        /// <summary>The pawn itself, held: once in a caravan it is on no map and no name lookup finds it.</summary>
        private sealed class AwayCelebrant
        {
            public Pawn Pawn;
        }

        private sealed class Newborn
        {
            public Pawn Pawn;
        }

        [Given("the celebrant is downed")]
        public void Down(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            HealthUtility.DamageUntilDowned(pawn, allowBleedingWounds: false);
            ctx.Require(pawn.Downed, $"{pawn.LabelShortCap} is still on their feet after DamageUntilDowned");
        }

        [When("the celebrant leaves the map in a caravan")]
        public void LeaveInCaravan(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            ctx.Set(new AwayCelebrant { Pawn = pawn });
            FormCaravan(ctx, new[] { pawn });
            ctx.Assert(!pawn.Spawned, $"{pawn.LabelShortCap} is still spawned on the map after the caravan formed");
        }

        /// <summary>
        /// "Witnesses" are counted the way GameComponent_Birthdays.CountOtherColonistsOnMap counts them:
        /// free colonists spawned on the map, other than the celebrant, babies excluded. The others
        /// leave in a caravan rather than being despawned, so the game keeps every pawn accounted for.
        /// </summary>
        [Given("all colonists but the celebrant and {int} other(s) have left the map in a caravan")]
        public void LeaveUntil(PickleContext ctx, int keep)
        {
            Pawn celebrant = Driver.Celebrant(ctx);
            var others = Driver.Map(ctx).mapPawns.FreeColonistsSpawned
                .Where(p => p != celebrant && !p.DevelopmentalStage.Baby())
                .ToList();
            ctx.Require(others.Count >= keep,
                $"only {others.Count} colonists besides the celebrant are on the map; this scenario keeps {keep}");

            var leaving = others.Skip(keep).ToList();
            if (leaving.Count > 0)
            {
                FormCaravan(ctx, leaving);
            }

            int now = Driver.Map(ctx).mapPawns.FreeColonistsSpawned
                .Count(p => p != celebrant && !p.DevelopmentalStage.Baby());
            ctx.Assert(now == keep, $"{now} other colonists remain on the map, expected {keep}");
        }

        [Then("the caravan celebrant holds no birthday-forgotten memory")]
        public void AssertAwayNoForgotten(PickleContext ctx) =>
            ctx.Assert(!Driver.HasMemoryOfDef(ctx.Get<AwayCelebrant>().Pawn, MHRDefOf.Nelim_BirthdayForgotten),
                "the celebrant, away in a caravan, holds a Nelim_BirthdayForgotten memory: "
                + "a pawn off the map cannot have been forgotten by the people around them");

        [Then("the caravan celebrant holds no birthday-remembered memory")]
        public void AssertAwayNoRemembered(PickleContext ctx) =>
            ctx.Assert(!Driver.HasMemoryOfDef(ctx.Get<AwayCelebrant>().Pawn, MHRDefOf.Nelim_BirthdayRemembered),
                "the celebrant, away in a caravan, holds a Nelim_BirthdayRemembered memory: "
                + "caravan pawns run no social interactions, so no verdict is owed either way");

        /// <summary>
        /// A baby is generated the way the game generates one (PawnGenerator, Baby developmental stage)
        /// and born today. Needs Biotech: without it there are no babies, and the feature that uses this
        /// carries @requires:ludeon.rimworld.biotech.
        /// </summary>
        [Given("a newborn colonist is born today next to the celebrant")]
        public void SpawnNewborn(PickleContext ctx)
        {
            Pawn celebrant = Driver.Celebrant(ctx);
            var request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer,
                forceGenerateNewPawn: true, fixedBiologicalAge: 0f, fixedChronologicalAge: 0f,
                developmentalStages: DevelopmentalStage.Baby);
            Pawn baby = PawnGenerator.GeneratePawn(request);

            IntVec3 cell = CellFinder.RandomClosewalkCellNear(celebrant.Position, Driver.Map(ctx), 3);
            GenSpawn.Spawn(baby, cell, Driver.Map(ctx));
            baby.ageTracker.BirthAbsTicks = Find.TickManager.TicksAbs;
            ctx.Set(new Newborn { Pawn = baby });
        }

        /// <summary>
        /// Both halves are asserted, so a pass cannot come from the newborn simply not being a colonist:
        /// it is a free colonist born on today's day of the year, and the mod's own rule still says no.
        /// </summary>
        [Then("the newborn's birthday is today but Many Happy Returns does not count it")]
        public void AssertNewbornExcluded(PickleContext ctx)
        {
            Pawn baby = ctx.Get<Newborn>().Pawn;
            ctx.Assert(baby.IsFreeColonist, "the generated newborn is not a free colonist, so this proves nothing");
            ctx.Assert(BirthdayUtility.IsBirthdayToday(baby), "the newborn's birth date is not today's day of the year");
            ctx.Assert(!BirthdayUtility.CanCelebrate(baby), "BirthdayUtility.CanCelebrate accepts a newborn");
        }

        [Then("Many Happy Returns does not track the newborn")]
        public void AssertNewbornNotTracked(PickleContext ctx)
        {
            Pawn baby = ctx.Get<Newborn>().Pawn;
            var field = Driver.Field(ctx, typeof(GameComponent_Birthdays), "celebratingToday", Driver.InstanceAny);
            var records = ((System.Collections.IEnumerable)field.GetValue(Driver.Component(ctx))).Cast<BirthdayRecord>();
            ctx.Assert(!records.Any(r => r.pawn == baby), "the newborn is on today's list of birthdays");
        }

        private static void FormCaravan(PickleContext ctx, System.Collections.Generic.IEnumerable<Pawn> pawns)
        {
            Map map = Driver.Map(ctx);
            Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, Faction.OfPlayer, map.Tile,
                map.Tile, PlanetTile.Invalid, false);
            ctx.Require(caravan != null, "CaravanExitMapUtility.ExitMapAndCreateCaravan formed no caravan");
        }
    }
}
