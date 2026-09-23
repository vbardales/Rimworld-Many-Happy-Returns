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
            var memory = Driver.MemoryOfDef(ctx, Driver.Celebrant(ctx), MHRDefOf.Nelim_BirthdayForgotten);
            AssertScaledByMoodFactor(ctx, memory);
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
        /// Forces the celebrant's TimeAsColonistOrColonyAnimal below the ten-day floor
        /// BirthdayUtility.MinTicksAsColonistForForgotten names, so the "recruit from yesterday"
        /// exemption (TEST_SCENARIOS.md S10) is exercised without waiting real days. Reverted after
        /// the scenario: see RestoreMutations.
        /// </summary>
        [Given("the celebrant has been a colonist for one day")]
        public void MakeRecruit(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            float original = pawn.records.GetValue(RecordDefOf.TimeAsColonistOrColonyAnimal);
            mutatedRecords.Add(new RestoreRecord { pawn = pawn, original = original });
            Driver.SetRecordValue(ctx, pawn, RecordDefOf.TimeAsColonistOrColonyAnimal, GenDate.TicksPerDay);
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
            foreach (RestoreRecord entry in mutatedRecords)
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
