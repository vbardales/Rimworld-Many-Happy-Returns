using UnityEngine;
using Verse;

namespace ManyHappyReturns
{
    public class ManyHappyReturnsMod : Mod
    {
        private static ManyHappyReturnsSettings settings;

        public ManyHappyReturnsMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ManyHappyReturnsSettings>();
        }

        /// <summary>
        /// Never null: the game builds every Mod before any def or component runs, but a fallback
        /// keeps a stray call during def loading from throwing instead of using the defaults.
        /// </summary>
        public static ManyHappyReturnsSettings Settings =>
            settings ?? (settings = new ManyHappyReturnsSettings());

        public override string SettingsCategory() => "Many Happy Returns";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            ManyHappyReturnsSettings s = Settings;
            s.moodFactor = s.ClampedMoodFactor;

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("ManyHappyReturns.Settings.Intro".Translate());
            listing.Label("ManyHappyReturns.Settings.Scope".Translate());
            listing.GapLine();

            listing.CheckboxLabeled(
                "ManyHappyReturns.Settings.MorningLetter".Translate(),
                ref s.morningLetter,
                "ManyHappyReturns.Settings.MorningLetterTip".Translate());

            listing.CheckboxLabeled(
                "ManyHappyReturns.Settings.ForgottenThought".Translate(),
                ref s.forgottenThought,
                "ManyHappyReturns.Settings.ForgottenThoughtTip".Translate());

            listing.Gap();
            Rect labelRect = listing.GetRect(Text.LineHeight);
            Widgets.Label(labelRect,
                "ManyHappyReturns.Settings.MoodFactor".Translate(Mathf.RoundToInt(s.moodFactor * 100f)));
            TooltipHandler.TipRegion(labelRect, "ManyHappyReturns.Settings.MoodFactorTip".Translate());
            s.moodFactor = Widgets.HorizontalSlider(
                listing.GetRect(22f),
                s.moodFactor,
                ManyHappyReturnsSettings.MinMoodFactor,
                ManyHappyReturnsSettings.MaxMoodFactor,
                middleAlignment: false,
                null,
                null,
                null,
                0.05f);

            listing.Gap();
            if (listing.ButtonText("ManyHappyReturns.Settings.Reset".Translate(), null, 0.35f))
            {
                s.Reset();
            }

            listing.End();
        }
    }
}
