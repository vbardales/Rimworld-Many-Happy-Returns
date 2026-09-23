using System;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorks.Pickle;
using Verse;
using Verse.AI.Group;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// The one place this suite reaches into Gifts and Birthdays (KrukuCoB.rout), for S13. Nothing
    /// here references that mod at compile time: the minimal pass loads this assembly without it, and a
    /// bound reference would fail to load the suite in the very pass meant to prove Many Happy
    /// Returns stands alone. Its types are found by name at run time, and every feature that uses these
    /// steps carries @requires:KrukuCoB.rout so it skips, rather than fails, when the mod is absent.
    ///
    /// Its party is a real LordJob_BirthdayParty, built with the constructor the mod's own
    /// BirthdayPartyUtility.TryStartBirthdayParty uses, and ended through the vanilla ApplyOutcome
    /// that every LordJob_Joinable_Party ends with. What is skipped is the organiser search, the
    /// letter, and the five to fifteen thousand ticks a party lasts: none of them is what Many Happy
    /// Returns depends on.
    /// </summary>
    [PickleSteps]
    public class GiftsSteps
    {
        private const string CongratulationDef = "BirthdayCongratulation";
        private const string CongratulationThought = "BirthdayCongratulationReceived";
        private const string PartyJobType = "Rout.LordJob_BirthdayParty";

        private sealed class DayBonusNote
        {
            public int Value;
        }

        [When("Gifts and Birthdays congratulates the celebrant from the wisher")]
        public void Congratulate(PickleContext ctx)
        {
            InteractionDef def = DefDatabase<InteractionDef>.GetNamedSilentFail(CongratulationDef);
            ctx.Require(def != null,
                $"no InteractionDef named '{CongratulationDef}': Gifts and Birthdays is not loaded, or renamed it");

            bool ok = Driver.Wisher(ctx).interactions.TryInteractWith(Driver.Celebrant(ctx), def);
            ctx.Assert(ok, "Pawn_InteractionsTracker.TryInteractWith refused Gifts and Birthdays' congratulation: "
                + "the pair is out of range, or one of them interacted less than 120 ticks ago");
        }

        [Then("the celebrant holds a Gifts and Birthdays congratulation from the wisher")]
        public void AssertCongratulation(PickleContext ctx)
        {
            ThoughtDef def = DefDatabase<ThoughtDef>.GetNamedSilentFail(CongratulationThought);
            ctx.Require(def != null, $"no ThoughtDef named '{CongratulationThought}'");

            var memory = Driver.MemoryOfDef(ctx, Driver.Celebrant(ctx), def);
            ctx.Assert(memory.otherPawn == Driver.Wisher(ctx),
                $"the congratulation names {memory.otherPawn?.LabelShortCap ?? "nobody"}, not the wisher");
        }

        [Given("a Gifts and Birthdays party is held for the celebrant, organised by the wisher")]
        public void StartParty(PickleContext ctx)
        {
            Type jobType = GenTypes.GetTypeInAnyAssembly(PartyJobType);
            ctx.Require(jobType != null, $"no type named {PartyJobType}: Gifts and Birthdays is not loaded, or renamed it");

            Pawn celebrant = Driver.Celebrant(ctx);
            Pawn wisher = Driver.Wisher(ctx);
            var job = (LordJob)Activator.CreateInstance(jobType,
                new object[] { celebrant.Position, wisher, GatheringDefOf.Party, celebrant });
            Lord lord = LordMaker.MakeNewLord(wisher.Faction, job, Driver.Map(ctx), null);

            foreach (Pawn pawn in new[] { wisher, celebrant })
            {
                if (!lord.ownedPawns.Contains(pawn))
                {
                    lord.AddPawn(pawn);
                }
            }
        }

        /// <summary>
        /// The claim in the description - "its party is recognised as a party" - rests on one thing: the
        /// thought that party hands its attendees is the vanilla AttendedParty, the def
        /// BirthdayUtility.DayQualityBonus looks for by name. LordJob_Joinable_Party.AttendeeThought is
        /// protected and virtual, so a subclass could have replaced it; the live job is asked.
        /// </summary>
        [Then("the party's attendee thought is the one Many Happy Returns reads as a party")]
        public void AssertAttendeeThought(PickleContext ctx)
        {
            LordJob job = PartyLord(ctx).LordJob;
            var property = Driver.Property(ctx, typeof(LordJob_Joinable_Party), "AttendeeThought", Driver.InstanceAny);
            var thought = (ThoughtDef)property.GetValue(job);

            ThoughtDef expected = DefDatabase<ThoughtDef>.GetNamedSilentFail("AttendedParty");
            ctx.Assert(thought == expected,
                $"the party grants {thought?.defName ?? "nothing"}, not {expected?.defName}: "
                + "DayQualityBonus would not see it as a party");
        }

        [When("the Gifts and Birthdays party is played out with everyone present throughout")]
        public void PlayOut(PickleContext ctx)
        {
            Lord lord = PartyLord(ctx);
            var job = (LordJob_Joinable_Party)lord.LordJob;
            var toil = lord.CurLordToil as LordToil_Party;
            ctx.Require(toil != null, "the party's current toil is not a LordToil_Party");
            var data = toil.data as LordToilData_Gathering;
            ctx.Require(data != null, "the party toil carries no LordToilData_Gathering");

            foreach (Pawn pawn in lord.ownedPawns)
            {
                data.presentForTicks[pawn] = job.DurationTicks;
            }

            Driver.Method(ctx, typeof(LordJob_Joinable_Party), "ApplyOutcome",
                BindingFlags.Instance | BindingFlags.NonPublic).Invoke(job, new object[] { toil });
        }

        [Then("the celebrant holds a party memory")]
        public void AssertPartyMemory(PickleContext ctx)
        {
            ThoughtDef party = DefDatabase<ThoughtDef>.GetNamedSilentFail("AttendedParty");
            ctx.Require(party != null, "no ThoughtDef named AttendedParty");
            Driver.MemoryOfDef(ctx, Driver.Celebrant(ctx), party);
        }

        [Given("Many Happy Returns notes the celebrant's day-quality bonus")]
        public void NoteBonus(PickleContext ctx) =>
            ctx.Set(new DayBonusNote { Value = BirthdayUtility.DayQualityBonus(Driver.Celebrant(ctx)) });

        [Then("the celebrant's day-quality bonus is {int} higher than noted")]
        public void AssertBonusRose(PickleContext ctx, int expected)
        {
            int before = ctx.Get<DayBonusNote>().Value;
            int now = BirthdayUtility.DayQualityBonus(Driver.Celebrant(ctx));
            ctx.Assert(now - before == expected,
                $"the day-quality bonus went from {before} to {now}, a rise of {now - before}, expected {expected}");
        }

        /// <summary>
        /// Recomputed from the same mod functions the verdict uses, over the same memories, so the
        /// assertion is "the memory carries the stage the day's own tally gives" and not a number
        /// copied from the scoring table - which Tests/Program.cs already proves offline.
        /// </summary>
        [Then("the celebrant's remembered memory is at the stage the day's own tally gives")]
        public void AssertStageMatchesTally(PickleContext ctx)
        {
            Pawn celebrant = Driver.Celebrant(ctx);
            int wishers = BirthdayUtility.CountWishers(celebrant, out bool close);
            int score = BirthdayUtility.PointsForWishers(wishers) + (close ? 2 : 0) + BirthdayUtility.DayQualityBonus(celebrant);
            int expected = BirthdayUtility.StageForScore(score);

            var memory = Driver.MemoryOfDef(ctx, celebrant, MHRDefOf.Nelim_BirthdayRemembered);
            ctx.Assert(memory.CurStageIndex == expected,
                $"the remembered memory is at stage {memory.CurStageIndex}; the day's tally "
                + $"({wishers} wisher(s), close={close}, score {score}) gives stage {expected}");
        }

        private static Lord PartyLord(PickleContext ctx)
        {
            Lord lord = Driver.Map(ctx).lordManager.lords
                .FirstOrDefault(l => l.LordJob != null && l.LordJob.GetType().FullName == PartyJobType);
            ctx.Require(lord != null, $"no lord running a {PartyJobType} on this map");
            return lord;
        }
    }
}
