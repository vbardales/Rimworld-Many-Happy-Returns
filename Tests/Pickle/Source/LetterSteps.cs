using System.Linq;
using System.Threading.Tasks;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// The morning letter. Matched by rebuilding the exact line from the mod's own translation key
    /// with the same arguments the mod passes, never on English text: the same steps hold in the
    /// French pass, where a missing key would come back as accented gibberish rather than pass
    /// quietly. See BillAutopilot's LetterSteps.cs, which this mirrors.
    /// </summary>
    [PickleSteps]
    public class LetterSteps
    {
        [When("the celebrant's birthday is moved to today")]
        public void MoveToToday(PickleContext ctx) => Driver.MoveBirthdayToToday(ctx, Driver.Celebrant(ctx));

        [When("Many Happy Returns rescans for birthdays")]
        public void Rescan(PickleContext ctx) => Driver.Component(ctx).DebugRescanNow();

        [Then("a birthday letter for the celebrant is on the stack")]
        public async Task AssertLetter(PickleContext ctx)
        {
            string label = LabelFor(ctx);
            await ctx.WaitUntil(() => Letters().Any(l => l.Label.Resolve() == label), 10f);

            ctx.Assert(Letters().Any(l => l.Label.Resolve() == label),
                $"no letter labelled '{label}' on the stack. The stack holds: " + Describe());
        }

        /// <summary>
        /// Counts rather than asserting existence, so a scenario that rescans twice can show the
        /// letter was queued exactly once and not once per rescan (TEST_SCENARIOS.md S04).
        /// </summary>
        [Then("exactly {int} birthday letter(s) for the celebrant are on the stack")]
        public void AssertLetterCount(PickleContext ctx, int expected)
        {
            string label = LabelFor(ctx);
            int actual = Letters().Count(l => l.Label.Resolve() == label);

            ctx.Assert(actual == expected,
                $"expected {expected} letter(s) labelled '{label}', found {actual}. "
                + "The stack holds: " + Describe());
        }

        internal static string LabelFor(PickleContext ctx)
        {
            Pawn pawn = Driver.Celebrant(ctx);
            return "ManyHappyReturns.BirthdayLetterLabel".Translate(pawn.Named("PAWN")).Resolve();
        }

        internal static System.Collections.Generic.List<Letter> Letters() =>
            Find.LetterStack?.LettersListForReading ?? new System.Collections.Generic.List<Letter>();

        private static string Describe()
        {
            var letters = Letters();
            return letters.Count == 0
                ? "nothing"
                : string.Join(" | ", letters.Select(l => l.Label.Resolve()).ToArray());
        }
    }
}
