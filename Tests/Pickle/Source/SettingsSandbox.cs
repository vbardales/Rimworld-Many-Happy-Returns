using System.IO;
using Verse;
using RimWorks.Pickle;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// Every settings-changing step in this suite mutates ManyHappyReturnsSettings live, the same
    /// object DoSettingsWindowContents and GameComponent_Birthdays both read. Left alone that would
    /// leak into the player's own configuration file the moment anything calls WriteSettings (which
    /// Dialog_ModSettings does when it closes). Follows the collection's established SettingsSandbox
    /// pattern (SkillIcons, ArchitectStudio, WorkStudio, Housebroken, ContentedLivestock,
    /// FieldworkCompanions): back up the real file before a scenario, restore it after. If the game
    /// died mid-scenario, the next run's IsolateSettings restores the backup first.
    /// </summary>
    [PickleSteps]
    public class SettingsSandbox
    {
        private static Mod ModInstance(PickleContext ctx) => Driver.Mod(ctx);

        private static string SettingsPath(PickleContext ctx)
        {
            var mod = ModInstance(ctx);
            var method = Driver.Method(ctx, typeof(LoadedModManager), "GetSettingsFilename", Driver.StaticAny);
            return (string)method.Invoke(null, new object[] { mod.Content.FolderName, mod.GetType().Name });
        }

        private static string BackupPath(PickleContext ctx) => SettingsPath(ctx) + ".pickle-backup";

        [BeforeScenario]
        public void IsolateSettings(PickleContext ctx)
        {
            if (File.Exists(BackupPath(ctx)))
            {
                // Left behind by a run that never finished: the backup is the player's real file.
                RestoreFromBackup(ctx);
            }

            ModInstance(ctx).WriteSettings();
            if (File.Exists(SettingsPath(ctx)))
            {
                File.Copy(SettingsPath(ctx), BackupPath(ctx), overwrite: false);
            }
        }

        [AfterScenario]
        public void RestoreSettings(PickleContext ctx)
        {
            if (File.Exists(BackupPath(ctx)))
            {
                RestoreFromBackup(ctx);
            }
        }

        /// <summary>
        /// The documented defaults: a brand new instance already carries them via field initializers
        /// (morningLetter=true, forgottenThought=true, moodFactor=1), the same values
        /// Tests/Program.cs's "Clean settings defaults" case checks against the compiled DLL.
        /// </summary>
        [Given("Many Happy Returns settings are at their defaults")]
        public void ResetToDefaults(PickleContext ctx)
        {
            var settings = new ManyHappyReturnsSettings();
            Adopt(ctx, settings);
            Driver.Field(ctx, typeof(ManyHappyReturnsMod), "settings", Driver.StaticAny).SetValue(null, settings);
            Driver.Field(ctx, typeof(Mod), "modSettings", Driver.InstanceAny).SetValue(ModInstance(ctx), settings);
        }

        /// <summary>
        /// ModSettings carries a reference back to the Mod that owns it, set by the game in
        /// GetSettings&lt;T&gt;(). An object built here with `new` has none, and ModSettings.Write()
        /// dereferences it: every scenario that wrote settings would die with "Object reference not
        /// set to an instance of an object", including one that only closes Dialog_ModSettings, since
        /// the dialog writes as it closes. Adopt the object before anything is allowed to hold it.
        /// </summary>
        private static void Adopt(PickleContext ctx, ModSettings settings)
        {
            // ModSettings.Mod is a property with a non-public setter, not a field: a field lookup
            // named "mod" finds nothing here. Checked against the shipped assembly, following the
            // same fix SkillIcons' SettingsSandbox needed for the same class.
            var property = Driver.Property(ctx, typeof(ModSettings), "Mod", Driver.InstanceAny);
            property.SetValue(settings, ModInstance(ctx), null);
        }

        private static void RestoreFromBackup(PickleContext ctx)
        {
            File.Copy(BackupPath(ctx), SettingsPath(ctx), overwrite: true);
            File.Delete(BackupPath(ctx));

            var mod = ModInstance(ctx);
            // Forces the next GetSettings<T>() to actually reload from the file just restored,
            // instead of handing back the in-memory object a scenario mutated.
            Driver.Field(ctx, typeof(Mod), "modSettings", Driver.InstanceAny).SetValue(mod, null);
            var settings = mod.GetSettings<ManyHappyReturnsSettings>();
            Driver.Field(ctx, typeof(ManyHappyReturnsMod), "settings", Driver.StaticAny).SetValue(null, settings);
        }
    }
}
