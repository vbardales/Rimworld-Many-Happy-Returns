using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// What only a running game proves about settings: that the primary Mod options route and the
    /// hidden shortcut both build a real Dialog_ModSettings for this mod's own live object, that
    /// closing it actually writes the file through the game's own Scribe (Dialog_ModSettings.PreClose
    /// calling Mod.WriteSettings), and that a freshly re-read Mod.GetSettings&lt;T&gt;() - not the
    /// in-memory object a scenario just mutated - agrees with what was written.
    ///
    /// Everything about the values themselves - defaults, reset, clamping of NaN/Infinity/out-of-
    /// range, and a headless Scribe round trip against the mod's actual LoadingVars/PostLoadInit
    /// callbacks - is already proven offline in Tests/Program.cs and not repeated here.
    /// </summary>
    [PickleSteps]
    public class SettingsSteps
    {
        [Given("the primary settings page for Many Happy Returns is opened")]
        public void OpenPrimary(PickleContext ctx) => Find.WindowStack.Add(new Dialog_ModSettings(Driver.Mod(ctx)));

        [When("the announce-birthdays setting is switched off")]
        public void SwitchOffAnnounce(PickleContext ctx) => Driver.Settings(ctx).morningLetter = false;

        [When("the settings dialog is closed")]
        public void CloseDialog(PickleContext ctx)
        {
            var dialog = Find.WindowStack.WindowOfType<Dialog_ModSettings>();
            ctx.Require(dialog != null, "no Dialog_ModSettings is open to close");
            Find.WindowStack.TryRemove(dialog);
        }

        /// <summary>
        /// Reads the file fresh: nulls the mod's cached ModSettings field first, exactly as
        /// SettingsSandbox.RestoreFromBackup does, so GetSettings&lt;T&gt;() is forced to deserialise
        /// the file just written rather than hand back the object the previous step already mutated.
        /// That difference is the entire point of this step; without it the assertion would pass even
        /// if PreClose had never called WriteSettings at all.
        /// </summary>
        [Then("Many Happy Returns's settings file on disk records the announce-birthdays setting as off")]
        public void AssertWrittenToDisk(PickleContext ctx)
        {
            var mod = Driver.Mod(ctx);
            Driver.Field(ctx, typeof(Mod), "modSettings", Driver.InstanceAny).SetValue(mod, null);
            var reloaded = mod.GetSettings<ManyHappyReturnsSettings>();
            Driver.Field(ctx, typeof(ManyHappyReturnsMod), "settings", Driver.StaticAny).SetValue(null, reloaded);

            ctx.Assert(!reloaded.morningLetter,
                "the settings file re-read from disk still has morningLetter=true: closing the dialog "
                + "did not persist the change, or PreClose did not call WriteSettings");
        }

        /// <summary>
        /// What the primary route shows after a change made through the shortcut. Both routes hold the
        /// one Mod instance, so this reads the value the second dialog would draw (TEST_SCENARIOS.md S12:
        /// "edit settings and verify shared values").
        /// </summary>
        [Then("the announce-birthdays setting reads off")]
        public void AssertAnnounceOff(PickleContext ctx) =>
            ctx.Assert(!Driver.Settings(ctx).morningLetter,
                "the announce-birthdays setting still reads on after it was switched off through the other route");
    }
}
