using System.Linq;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// The optional MainButtons shortcut, and the contract MOD_SETTINGS.md puts on it: available for
    /// RIMMSQOL and other customization tools to reveal, hidden by default, neither visible nor
    /// greyed, and opening the same settings as Mod options.
    ///
    /// What a customization mod does when a player reveals the button is move
    /// <c>MainButtonDef.buttonVisible</c>. What this mod owes is the other side of that contract, so
    /// these steps move that same field and then ask RimWorld's own worker what the bar would do.
    /// Nothing here installs or drives RIMMSQOL: whether ITS interface can reveal the button, and
    /// whether ITS choice survives a restart, is RIMMSQOL's own behaviour and stays manual
    /// (TEST_SCENARIOS.md S12), exactly as Bill Autopilot's ShortcutSteps.cs draws the same line.
    /// </summary>
    [PickleSteps]
    public class ShortcutSteps
    {
        private const string DefName = "Nelim_ManyHappyReturnsSettings";

        [Then("Many Happy Returns's settings shortcut is hidden on a clean configuration")]
        public void AssertHiddenByDefault(PickleContext ctx)
        {
            var def = Shortcut(ctx);
            ctx.Assert(!def.buttonVisible,
                "Nelim_ManyHappyReturnsSettings ships with buttonVisible true: it would stand in "
                + "everyone's main bar without anyone asking for it");
            AssertNotDrawn(ctx);
        }

        [When("Many Happy Returns's settings shortcut is revealed, as a customization mod would")]
        public void Reveal(PickleContext ctx) => Shortcut(ctx).buttonVisible = true;

        [When("Many Happy Returns's settings shortcut is hidden again")]
        public void Hide(PickleContext ctx) => Shortcut(ctx).buttonVisible = false;

        /// <summary>
        /// Both halves are asserted. Worker.Visible decides whether the bar draws the def at all and
        /// Worker.Disabled decides whether it draws it greyed; MOD_SETTINGS.md forbids a greyed
        /// shortcut as firmly as a visible one, and a def can be drawn and still be dead.
        /// </summary>
        [Then("Many Happy Returns's settings shortcut is drawn in the bar")]
        public void AssertDrawn(PickleContext ctx)
        {
            var def = Shortcut(ctx);
            ctx.Assert(def.Worker.Visible,
                "the shortcut has been revealed and its worker still reports Visible false, so a "
                + "customization mod cannot actually put it in the bar");
            ctx.Assert(!def.Worker.Disabled,
                "the shortcut is drawn but greyed out, which MOD_SETTINGS.md forbids");
        }

        [Then("Many Happy Returns's settings shortcut is not drawn in the bar")]
        public void AssertNotDrawn(PickleContext ctx)
        {
            var def = Shortcut(ctx);
            ctx.Assert(!def.Worker.Visible,
                $"the shortcut reports Visible true with buttonVisible {def.buttonVisible}: it shows "
                + "without anything having revealed it");
        }

        /// <summary>
        /// Activating the worker is what a revealed button ends up calling. The claim being checked
        /// is not "a settings window opened" but "the SAME settings opened": a dialog belonging to
        /// another mod would look identical in a screenshot, so the window is asked which mod it is
        /// for.
        /// </summary>
        [When("Many Happy Returns's settings shortcut is activated")]
        public void Activate(PickleContext ctx) => Shortcut(ctx).Worker.Activate();

        [Then("a settings dialog is open for Many Happy Returns")]
        public void AssertDialogOpen(PickleContext ctx)
        {
            var stack = Find.WindowStack;
            ctx.Require(stack != null, "there is no window stack: no game and no main menu is running");

            var dialogs = stack.Windows.OfType<Dialog_ModSettings>().ToList();
            ctx.Assert(dialogs.Count > 0,
                "no Dialog_ModSettings is open: activating the shortcut opened nothing at all");

            var mine = Driver.Mod(ctx);
            bool found = dialogs.Any(d => ModOf(ctx, d) == mine);
            ctx.Assert(found,
                "a settings dialog is open, but not this mod's: it was built for "
                + string.Join(", ", dialogs.Select(d => ModOf(ctx, d)?.Content?.Name ?? "an unknown mod").ToArray())
                + ". The shortcut and Mod options must lead to the same place");
        }

        private static MainButtonDef Shortcut(PickleContext ctx)
        {
            var def = DefDatabase<MainButtonDef>.GetNamedSilentFail(DefName);
            ctx.Require(def != null,
                $"no MainButtonDef named '{DefName}': the shortcut a customization mod is meant to be "
                + "able to reveal is not shipped at all");
            return def;
        }

        /// <summary>
        /// Dialog_ModSettings keeps the mod it was built for in a private field. The name has moved
        /// between game versions, so both spellings are tried and the failure names the fields the
        /// class really has rather than reporting a null reference. Mirrors Bill Autopilot's own
        /// ShortcutSteps.cs, which found both spellings across versions.
        /// </summary>
        private static Mod ModOf(PickleContext ctx, Dialog_ModSettings dialog)
        {
            var type = typeof(Dialog_ModSettings);
            var field = type.GetField("mod", Driver.InstanceAny) ?? type.GetField("selMod", Driver.InstanceAny);

            ctx.Require(field != null,
                "Dialog_ModSettings has neither a 'mod' nor a 'selMod' field in this version; it has: "
                + string.Join(", ", type.GetFields(Driver.InstanceAny | System.Reflection.BindingFlags.Public)
                    .Select(f => f.Name).ToArray()));

            return field.GetValue(dialog) as Mod;
        }
    }
}
