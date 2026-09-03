using RimWorld;
using Verse;

namespace ManyHappyReturns
{
    [DefOf]
    public static class MHRDefOf
    {
        public static InteractionDef Nelim_BirthdayWish;

        public static ThoughtDef Nelim_BirthdayWishReceived;

        public static ThoughtDef Nelim_BirthdayWishGiven;

        public static ThoughtDef Nelim_BirthdayRemembered;

        public static ThoughtDef Nelim_BirthdayForgotten;

        static MHRDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MHRDefOf));
        }
    }
}
