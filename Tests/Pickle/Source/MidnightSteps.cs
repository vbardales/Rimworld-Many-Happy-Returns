using System.Collections;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// The day closing on its own (TEST_SCENARIOS.md S11). The verdict is formed by
    /// GameComponent_Birthdays when the absolute day changes, not by the debug action the other
    /// features use, and the game clock is the only thing that can prove that path.
    ///
    /// The clock is placed with TickManager.DebugSetTicksGame, the same call the game's own
    /// "Increment time" makes. The mod's debug notes are right that it is useless for a birthday
    /// (nobody gets the time to wish anybody anything), so it is used here only to POSITION the clock a few
    /// hundred ticks short of a boundary; everything after it is played tick by tick with
    /// "I wait N ticks". The component then sees one day change caused by the jump itself, which is why
    /// the features wait a few ticks before arranging the birthday.
    /// </summary>
    [PickleSteps]
    public class MidnightSteps
    {
        [Given("the game clock is set to {int} ticks before midnight")]
        public void SetBeforeMidnight(PickleContext ctx, int ticks) => Jump(ctx, GenDate.TicksPerDay, ticks);

        /// <summary>
        /// DayOfYear is read at longitude 0 by the mod, so the year turns when the absolute tick count
        /// crosses a multiple of GenDate.TicksPerYear, whatever the map's own longitude.
        /// </summary>
        [Given("the game clock is set to {int} ticks before the end of the year")]
        public void SetBeforeNewYear(PickleContext ctx, int ticks) => Jump(ctx, GenDate.TicksPerYear, ticks);

        /// <summary>
        /// The morning letter is dropped, not queued, past hour 20 local (GameComponent_Birthdays.
        /// LatestLetterHour), and the map's longitude decides what local time a given absolute tick is.
        /// A scenario that asserts the letter therefore fixes the local hour first, and only ever moves
        /// the clock forward.
        /// </summary>
        [Given("the local hour at the celebrant is set to {int}")]
        public void SetLocalHour(PickleContext ctx, int hour)
        {
            var manager = Find.TickManager;
            ctx.Require(manager != null, "no TickManager: this step needs a loaded game");
            int current = GenLocalDate.HourOfDay(Driver.Celebrant(ctx));
            int hours = (hour - current + 24) % 24;
            manager.DebugSetTicksGame(manager.TicksGame + hours * GenDate.TicksPerHour);
        }

        [Then("Many Happy Returns is tracking {int} birthday(s) today")]
        public void AssertTracked(PickleContext ctx, int expected)
        {
            var field = Driver.Field(ctx, typeof(GameComponent_Birthdays), "celebratingToday", Driver.InstanceAny);
            var list = (IList)field.GetValue(Driver.Component(ctx));
            ctx.Assert(list.Count == expected,
                $"GameComponent_Birthdays tracks {list.Count} birthday(s) today, expected {expected}");
        }

        /// <summary>
        /// The birthday is not the pawn's any more once the day has turned: the calendar day the mod
        /// compares against BirthDayOfYear is a different one.
        /// </summary>
        [Then("it is no longer the celebrant's birthday")]
        public void AssertNotToday(PickleContext ctx) =>
            ctx.Assert(!BirthdayUtility.IsBirthdayToday(Driver.Celebrant(ctx)),
                "BirthdayUtility.IsBirthdayToday still holds for the celebrant after midnight");

        private static void Jump(PickleContext ctx, int period, int ticksBefore)
        {
            var manager = Find.TickManager;
            ctx.Require(manager != null, "no TickManager: this step needs a loaded game");
            ctx.Require(ticksBefore > 0 && ticksBefore < period, $"{ticksBefore} ticks is not inside one period of {period}");

            long target = period - ticksBefore;
            long delta = ((target - manager.TicksAbs % period) + period) % period;
            manager.DebugSetTicksGame(manager.TicksGame + (int)delta);
        }
    }
}
