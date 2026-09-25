using System.Collections;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// What 15-publication-shots.feature needs on top of the steps the other features already have:
    /// the pawn in the inspect pane, the camera on the pawn, the letter opened, a settings value a
    /// player would have chosen, and a birthday wished by everybody who could. The scene is built by
    /// the scenario, in the showcase colony, so that nothing on the page is the fixture's debris.
    /// </summary>
    [PickleSteps]
    public class PublicationSteps
    {
        [When("I select the celebrant")]
        public void SelectCelebrant(PickleContext ctx) => Select(Driver.Celebrant(ctx));

        [When("the camera is centred on the celebrant")]
        public void CentreOnCelebrant(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            Find.CameraDriver.JumpToCurrentMapLoc(pawn.Position);
        }

        /// <summary>
        /// The mod's own debug actions confirm what they did with an on-screen message ("Birthday moved
        /// to today"). It is right for a developer and wrong on a store page, so a capture that keeps the
        /// interface clears it first. Messages has no public way to do that: its live list is emptied.
        /// </summary>
        [When("the on-screen messages are cleared")]
        public void ClearMessages(PickleContext ctx)
        {
            FieldInfo live = Driver.Field(ctx, typeof(Messages), "liveMessages", Driver.StaticAny);
            (live.GetValue(null) as IList)?.Clear();
        }

        [When("I open the birthday letter for the celebrant")]
        public void OpenLetter(PickleContext ctx)
        {
            string label = LetterSteps.LabelFor(ctx);
            Letter letter = LetterSteps.Letters().FirstOrDefault(l => l.Label.Resolve() == label);
            ctx.Require(letter != null, $"no letter labelled '{label}' on the stack to open");
            letter.OpenLetter();
        }

        [Then("the letter window is open")]
        public void AssertLetterWindow(PickleContext ctx) =>
            ctx.Assert(Find.WindowStack.WindowOfType<Dialog_NodeTree>() != null,
                "opening the letter added no Dialog_NodeTree to the window stack");

        /// <summary>
        /// A value a player would have chosen, so the settings page shows the slider away from its
        /// default. The settings sandbox of "Many Happy Returns settings are at their defaults" puts the
        /// real file back after the scenario.
        /// </summary>
        [Given("the mood scale is set to {int} percent")]
        public void SetMoodScale(PickleContext ctx, int percent) =>
            Driver.Settings(ctx).moodFactor = percent / 100f;

        /// <summary>
        /// Every other free colonist walks up to the celebrant and wishes them well, through the same
        /// interaction the game runs for any social exchange: as many wishers as the colony holds.
        /// </summary>
        [When("every other colonist wishes the celebrant a happy birthday")]
        public void EveryoneWishes(PickleContext ctx)
        {
            Pawn celebrant = Driver.Celebrant(ctx);
            var others = Driver.Map(ctx).mapPawns.FreeColonistsSpawned
                .Where(p => p != celebrant && !p.Downed && p.RaceProps.Humanlike).ToList();
            ctx.Require(others.Count > 0, "no other colonist on the map to wish the celebrant a happy birthday");

            foreach (Pawn wisher in others)
            {
                Driver.BringTogether(ctx, wisher, celebrant);
                WishSteps.ExchangeWish(ctx, wisher, celebrant);
            }
        }

        private static void Select(Pawn pawn)
        {
            Find.Selector.ClearSelection();
            Find.Selector.Select(pawn, playSound: false, forceDesignatorDeselect: true);
        }
    }
}
