using Verse;

namespace ManyHappyReturns
{
    /// <summary>
    /// One colonist whose birthday is today. Deliberately a top-level class and not nested in
    /// GameComponent_Birthdays: Scribe writes a deep-saved element by type name, and a nested type
    /// only round-trips through the "Outer+Inner" reflection form. Top level, no question.
    /// </summary>
    public class BirthdayRecord : IExposable
    {
        public Pawn pawn;

        public bool letterSent;

        public void ExposeData()
        {
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_Values.Look(ref letterSent, "letterSent", defaultValue: false);
        }
    }
}
