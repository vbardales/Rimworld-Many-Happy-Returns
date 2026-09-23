using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ManyHappyReturns.PickleSteps
{
    /// <summary>
    /// What only a running game proves about loading: that MHRDefOf's static constructor actually
    /// resolved every def it names (DefOfHelper.EnsureInitializedInCtor throws otherwise, which an
    /// offline test cannot reach without the real def database), and that the optional lookup of
    /// Gifts and Birthdays' congratulation thought agrees with whether that mod is really loaded.
    /// Everything else about loading - XML validity, def references, DefInjected paths - is proven
    /// offline by Check-DefRefs.ps1, Check-XmlFields.ps1 and Check-DefInjected.ps1, and not repeated
    /// here.
    /// </summary>
    [PickleSteps]
    public class LoadingSteps
    {
        [Then("Many Happy Returns's own defs are all resolved")]
        public void AssertDefOfResolved(PickleContext ctx)
        {
            ctx.Assert(MHRDefOf.Nelim_BirthdayWish != null, "MHRDefOf.Nelim_BirthdayWish is null");
            ctx.Assert(MHRDefOf.Nelim_BirthdayWishReceived != null, "MHRDefOf.Nelim_BirthdayWishReceived is null");
            ctx.Assert(MHRDefOf.Nelim_BirthdayWishGiven != null, "MHRDefOf.Nelim_BirthdayWishGiven is null");
            ctx.Assert(MHRDefOf.Nelim_BirthdayRemembered != null, "MHRDefOf.Nelim_BirthdayRemembered is null");
            ctx.Assert(MHRDefOf.Nelim_BirthdayForgotten != null, "MHRDefOf.Nelim_BirthdayForgotten is null");

            MainButtonDef shortcut = DefDatabase<MainButtonDef>.GetNamedSilentFail("Nelim_ManyHappyReturnsSettings");
            ctx.Assert(shortcut != null,
                "no MainButtonDef named Nelim_ManyHappyReturnsSettings: the settings shortcut is not shipped");
        }

        /// <summary>
        /// The one point of contact with Gifts and Birthdays: a lookup by def name, resolved once and
        /// cached. Correct in every pass, whether that mod is staged or not - the claim is a
        /// correspondence, not a fixed answer, exactly as BillAutopilot's StateSteps does for its own
        /// soft integrations.
        /// </summary>
        [Then("Many Happy Returns's optional Gifts and Birthdays lookup matches what is loaded")]
        public void AssertOptionalLookupMatchesModList(PickleContext ctx)
        {
            bool loaded = ModsConfig.IsActive("KrukuCoB.rout");
            bool found = BirthdayUtility.GiftsAndBirthdaysWishDef != null;

            ctx.Assert(loaded == found,
                $"KrukuCoB.rout loaded={loaded} but BirthdayCongratulationReceived resolved={found}: "
                + "either the optional mod stopped shipping that def under that name, or the lookup "
                + "did not run in this session");
        }
    }
}
