using System.Linq;
using System.Collections.Generic;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// What only a running game proves about the end-of-day verdict: that CloseOutDay, wired to real
    /// Pawn objects, actually forms the graded Nelim_BirthdayRemembered or the negative
    /// Nelim_BirthdayForgotten through the game's own MemoryThoughtHandler.TryGainMemory - which is
    /// where nullifyingTraits and nullifyingHediffs are enforced, and which an offline test cannot
    /// reach without a full Pawn (mood tracker, trait tracker, records tracker all wired together).
    ///
    /// The stage/score arithmetic itself (StageForScore, PointsForWishers, the five documented
    /// grades) is already exhaustively proven offline in Tests/Program.cs and is not repeated here:
    /// this class asserts that the real memory formed carries the stage the mod's own function
    /// already computed, and the real settings' mood factor - not a fixed number.
    /// </summary>
    [PickleSteps]
    public class VerdictSteps
    {
        private static readonly List<RestoreRecord> mutatedRecords = new List<RestoreRecord>();
        private static readonly List<Pawn> psychopathsAdded = new List<Pawn>();

        [When("Many Happy Returns closes out today's birthdays")]
        public void CloseOutNow(PickleContext ctx) => Driver.Component(ctx).DebugCloseOutNow();

        [Then("the celebrant holds a birthday-remembered memory")]
        public void AssertRemembered(PickleContext ctx)
        {
            var memory = Driver.MemoryOfDef(ctx, Driver.Celebrant(ctx), MHRDefOf.Nelim_BirthdayRemembered);
            AssertScaledByMoodFactor(ctx, memory);
        }

        [Then("the celebrant holds no birthday-remembered memory")]
        public void AssertNoRemembered(PickleContext ctx) =>
            ctx.Assert(!Driver.HasMemoryOfDef(Driver.Celebrant(ctx), MHRDefOf.Nelim_BirthdayRemembered),
                "the celebrant holds a Nelim_BirthdayRemembered memory, and this scenario expected none");

        [Then("the celebrant holds a birthday-forgotten memory")]
        public void AssertForgotten(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            if (!Driver.HasMemoryOfDef(pawn, MHRDefOf.Nelim_BirthdayForgotten))
            {
                // Say why, rather than leaving the reader to guess which of the mod's conditions failed.
                float days = pawn.records.GetValue(RecordDefOf.TimeAsColonistOrColonyAnimal) / GenDate.TicksPerDay;
                int others = pawn.Map == null ? 0 : pawn.Map.mapPawns.FreeColonistsSpawned
                    .Count(p => p != pawn && !p.DevelopmentalStage.Baby());
                ctx.Assert(false,
                    $"{pawn.LabelShortCap} holds no Nelim_BirthdayForgotten memory. The conditions: forgottenThought="
                    + $"{Driver.Settings(ctx).forgottenThought}, {days:0.0} days as a colonist (needs 10), {others} other "
                    + $"colonists on the map (needs 2), spawned={pawn.Spawned}, downed={pawn.Downed}. Today's memories: "
                    + string.Join(", ", Driver.TodaysMemories(pawn).Select(m => m.def.defName).ToArray()));
            }

            AssertScaledByMoodFactor(ctx, Driver.MemoryOfDef(ctx, pawn, MHRDefOf.Nelim_BirthdayForgotten));
        }

        [Then("the celebrant holds no birthday-forgotten memory")]
        public void AssertNoForgotten(PickleContext ctx) =>
            ctx.Assert(!Driver.HasMemoryOfDef(Driver.Celebrant(ctx), MHRDefOf.Nelim_BirthdayForgotten),
                "the celebrant holds a Nelim_BirthdayForgotten memory, and this scenario expected none");

        private static void AssertScaledByMoodFactor(PickleContext ctx, Thought_Memory memory)
        {
            float expected = Driver.Settings(ctx).ClampedMoodFactor;
            ctx.Assert(memory.moodPowerFactor == expected,
                $"the memory's moodPowerFactor is {memory.moodPowerFactor}, expected the current "
                + $"settings' clamped mood factor {expected}: the real Mod.Settings was not read when "
                + "the memory was formed, or a stale settings object leaked in");
        }

        /// <summary>
        /// Sets the celebrant's TimeAsColonistOrColonyAnimal to a number of days. Below ten it is the
        /// "recruit from yesterday" exemption (TEST_SCENARIOS.md S10); at or above it is the precondition
        /// S09 states ("long-established target") that the test-colony fixture does NOT satisfy on its
        /// own: the first run showed no forgotten memory for a celebrant nobody had wished, because
        /// the fixture is a young colony. Reverted after the scenario, newest change first.
        /// </summary>
        [Given("the celebrant has been a colonist for {int} day(s)")]
        public void SetTenure(PickleContext ctx, int days)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            float original = pawn.records.GetValue(RecordDefOf.TimeAsColonistOrColonyAnimal);
            mutatedRecords.Add(new RestoreRecord { pawn = pawn, original = original });
            Driver.SetRecordValue(ctx, pawn, RecordDefOf.TimeAsColonistOrColonyAnimal, days * GenDate.TicksPerDay);
        }

        /// <summary>
        /// Adds the Psychopath trait if the celebrant does not already carry it, so the nullifying-
        /// trait guard on Nelim_BirthdayForgotten - "a psychopath must still be able to enjoy a good
        /// birthday", per Mod/Defs/Birthday.xml's own comment - can be exercised. Looked up by
        /// defName, exactly as the mod's own nullifyingTraits entry does; TraitDefOf carries no
        /// Psychopath constant. Reverted after the scenario.
        /// </summary>
        [Given("the celebrant is a psychopath")]
        public void MakePsychopath(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            TraitDef psychopath = DefDatabase<TraitDef>.GetNamedSilentFail("Psychopath");
            ctx.Require(psychopath != null, "no TraitDef named 'Psychopath': vanilla renamed it");

            if (pawn.story.traits.HasTrait(psychopath))
            {
                return;
            }

            pawn.story.traits.GainTrait(new Trait(psychopath));
            psychopathsAdded.Add(pawn);
        }

        [AfterScenario]
        public void RestoreMutations(PickleContext ctx)
        {
            foreach (RestoreRecord entry in Enumerable.Reverse(mutatedRecords))
            {
                Driver.SetRecordValue(ctx, entry.pawn, RecordDefOf.TimeAsColonistOrColonyAnimal, entry.original);
            }
            mutatedRecords.Clear();

            foreach (Pawn pawn in psychopathsAdded)
            {
                TraitDef psychopath = DefDatabase<TraitDef>.GetNamedSilentFail("Psychopath");
                Trait trait = psychopath == null ? null : pawn.story.traits.GetTrait(psychopath);
                if (trait != null)
                {
                    pawn.story.traits.allTraits.Remove(trait);
                }
            }
            psychopathsAdded.Clear();
        }

        private struct RestoreRecord
        {
            public Pawn pawn;
            public float original;
        }
    }
}
