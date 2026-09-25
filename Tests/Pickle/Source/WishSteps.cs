using System.Reflection;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// What only a running game proves about the wish: the InteractionWorker's own weight, evaluated
    /// against real Pawn objects with real memory handlers (needs.mood.thoughts.memories, relations,
    /// developmental stage), and the real Pawn_InteractionsTracker.TryInteractWith callback the game
    /// itself uses to run any interaction - not a hand assembly of thoughts.
    ///
    /// TryInteractWith runs the worker's Interacted() unconditionally: it does not consult
    /// RandomSelectionWeight at all. So the duplicate-protection contract - "the pair does not get
    /// picked again by the natural interaction roulette" - is exactly the thing this suite reads the
    /// weight to prove, rather than exchanging the wish twice and hoping a second exchange is
    /// refused. BirthdayUtility.PointsForWishers, StageForScore and the plain scoring arithmetic are
    /// already exhaustively covered offline (Tests/Program.cs); they are not repeated here.
    /// </summary>
    [PickleSteps]
    public class WishSteps
    {
        /// <summary>
        /// TryInteractWith and RandomSelectionWeight both require the pair within six tiles with
        /// line of sight (SocialInteractionUtility.IsGoodPositionForInteraction); a fixture is a
        /// played colony and its colonists are not guaranteed to start there.
        /// </summary>
        [Given("the wisher is brought next to the celebrant")]
        public void BringTogether(PickleContext ctx) => Driver.BringTogether(ctx, Driver.Wisher(ctx), Driver.Celebrant(ctx));

        [Then("the wish's selection weight from the wisher to the celebrant is high")]
        public void AssertWeightHigh(PickleContext ctx)
        {
            float weight = Weight(ctx);
            ctx.Assert(weight > 0f,
                $"MHRDefOf.Nelim_BirthdayWish.Worker.RandomSelectionWeight returned {weight} for an "
                + "eligible wisher on the celebrant's birthday: the pair should be strongly favoured");
        }

        [Then("the wish's selection weight from the wisher to the celebrant is zero")]
        public void AssertWeightZero(PickleContext ctx)
        {
            float weight = Weight(ctx);
            ctx.Assert(weight == 0f,
                $"MHRDefOf.Nelim_BirthdayWish.Worker.RandomSelectionWeight returned {weight}, not 0, "
                + "after this pair already exchanged the wish today: duplicate protection did not disqualify it");
        }

        [When("the wisher exchanges the birthday wish with the celebrant")]
        public void Exchange(PickleContext ctx) => ExchangeWish(ctx, Driver.Wisher(ctx), Driver.Celebrant(ctx));

        internal static void ExchangeWish(PickleContext ctx, Pawn wisher, Pawn celebrant)
        {
            // The fixture's colonists chat on their own while it loads and settles, and TryInteractWith
            // refuses an initiator that interacted less than 120 ticks ago. That ambient chat is not what
            // is under test, so the initiator starts each exchange without one on record.
            ctx.Require(lastInteractionTime != null,
                "Pawn_InteractionsTracker has no private lastInteractionTime field: vanilla renamed it");
            lastInteractionTime.SetValue(wisher.interactions, -9999);

            bool ok = wisher.interactions.TryInteractWith(celebrant, MHRDefOf.Nelim_BirthdayWish);
            if (!ok)
            {
                InteractionDef wish = MHRDefOf.Nelim_BirthdayWish;
                ctx.Assert(false,
                    "Pawn_InteractionsTracker.TryInteractWith refused the birthday wish. What CanInteractNowWith reads: "
                    + $"celebrant spawned={celebrant.Spawned}, good position="
                    + $"{(wisher.Map != null && celebrant.Map == wisher.Map && SocialInteractionUtility.IsGoodPositionForInteraction(wisher, celebrant))}"
                    + $" (wisher {wisher.Position}, celebrant {celebrant.Position}), wisher can initiate="
                    + $"{SocialInteractionUtility.CanInitiateInteraction(wisher, wish)} (awake={wisher.Awake()}, "
                    + $"downed={wisher.Downed}, mental state={wisher.MentalStateDef?.defName ?? "none"}), celebrant can receive="
                    + $"{SocialInteractionUtility.CanReceiveInteraction(celebrant, wish)} (awake={celebrant.Awake()}, "
                    + $"downed={celebrant.Downed}, mental state={celebrant.MentalStateDef?.defName ?? "none"})");
            }
        }

        private static readonly FieldInfo lastInteractionTime =
            typeof(Pawn_InteractionsTracker).GetField("lastInteractionTime", BindingFlags.Instance | BindingFlags.NonPublic);

        [Then("the celebrant holds a birthday wish received from the wisher")]
        public void AssertReceived(PickleContext ctx)
        {
            var memory = Driver.MemoryOfDef(ctx, Driver.Celebrant(ctx), MHRDefOf.Nelim_BirthdayWishReceived);
            ctx.Assert(memory.otherPawn == Driver.Wisher(ctx),
                $"the celebrant's Nelim_BirthdayWishReceived memory names {memory.otherPawn?.LabelShortCap ?? "nobody"} "
                + "as the other pawn, not the wisher who exchanged it");
        }

        [Then("the wisher holds a birthday wish given to the celebrant")]
        public void AssertGiven(PickleContext ctx)
        {
            var memory = Driver.MemoryOfDef(ctx, Driver.Wisher(ctx), MHRDefOf.Nelim_BirthdayWishGiven);
            ctx.Assert(memory.otherPawn == Driver.Celebrant(ctx),
                $"the wisher's Nelim_BirthdayWishGiven memory names {memory.otherPawn?.LabelShortCap ?? "nobody"} "
                + "as the other pawn, not the celebrant it was exchanged with");
        }

        [Then("Many Happy Returns counts the celebrant as wished by {int} colonist(s) today")]
        public void AssertWisherCount(PickleContext ctx, int expected)
        {
            int actual = BirthdayUtility.CountWishers(Driver.Celebrant(ctx), out _);
            ctx.Assert(actual == expected,
                $"BirthdayUtility.CountWishers reports {actual} wisher(s) for the celebrant, expected {expected}");
        }

        private static float Weight(PickleContext ctx) =>
            MHRDefOf.Nelim_BirthdayWish.Worker.RandomSelectionWeight(Driver.Wisher(ctx), Driver.Celebrant(ctx));
    }
}
