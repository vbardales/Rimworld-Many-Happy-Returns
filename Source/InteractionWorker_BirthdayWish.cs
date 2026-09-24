using RimWorld;
using Verse;

namespace ManyHappyReturns
{
    /// <summary>
    /// Makes the birthday wish an ordinary social interaction. Pawn_InteractionsTracker draws from
    /// every InteractionDef by weight, so returning a large weight on the right day is enough: no
    /// job, no lord, no patch. The weight drops back to zero once the pair has exchanged it, which
    /// keeps one wish per pair and per birthday instead of a loop of them.
    /// </summary>
    public class InteractionWorker_BirthdayWish : InteractionWorker
    {
        /// <summary>
        /// Well above Chitchat and DeepTalk, so the first words between two colonists on a birthday
        /// are the birthday. It only ever competes against the ordinary small talk of one pair on
        /// one day, and it disqualifies itself right afterwards.
        /// </summary>
        private const float Weight = 30f;

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator == null || recipient == null || initiator == recipient)
            {
                return 0f;
            }

            // Cheapest and most discriminating test first: this runs for every interaction def, on
            // every pair, on every social tick of the game, and it is false on 59 days out of 60.
            if (!BirthdayUtility.IsBirthdayToday(recipient))
            {
                return 0f;
            }

            // Anyone in the colony can wish, prisoners aside; only a colonist has a birthday the
            // colony would keep track of.
            if (initiator.Faction != Faction.OfPlayer || initiator.IsPrisoner)
            {
                return 0f;
            }

            if (!BirthdayUtility.CanCelebrate(recipient))
            {
                return 0f;
            }

            if (BirthdayUtility.AlreadyWished(recipient, initiator))
            {
                return 0f;
            }

            // The wish disqualifies itself by the memory it leaves, so it must be certain that the
            // memory will actually be stored. MemoryThoughtHandler.TryGainMemory drops it silently
            // when CanGetThought refuses it (stage, gender, ...), or when the initiator falls outside the
            // social target filter (a nullifying trait only zeroes the mood offset) - and a wish that
            // leaves no trace would repeat forever.
            ThoughtDef received = MHRDefOf.Nelim_BirthdayWishReceived;
            if (!ThoughtUtility.CanGetThought(recipient, received))
            {
                return 0f;
            }

            if (!received.socialTargetDevelopmentalStageFilter.Has(initiator.DevelopmentalStage))
            {
                return 0f;
            }

            return Weight;
        }
    }
}
