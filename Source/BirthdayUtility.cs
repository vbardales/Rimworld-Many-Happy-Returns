using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ManyHappyReturns
{
    /// <summary>
    /// Everything the mod needs to know about a birthday, read from what the game already records.
    /// No patches: the wishes are counted from the celebrant's own social memories, and the rest of
    /// the day is read from vanilla memories the pawn formed on its own.
    /// </summary>
    public static class BirthdayUtility
    {
        /// <summary>
        /// A memory older than this cannot belong to today. Wishes are only ever given on a
        /// birthday, and a birthday lasts one day, so one day plus an hour of slack is exact.
        /// </summary>
        public const int TodayWindowTicks = GenDate.TicksPerDay + GenDate.TicksPerHour;

        /// <summary>Ten days in the colony before a forgotten birthday can hurt.</summary>
        public const int MinTicksAsColonistForForgotten = 10 * GenDate.TicksPerDay;

        /// <summary>Opinion at which a colonist counts as a close friend for the day's score.</summary>
        public const int CloseFriendOpinion = 40;

        private static bool interopResolved;
        private static ThoughtDef giftsAndBirthdaysWishDef;

        private static bool vanillaResolved;
        private static ThoughtDef attendedPartyDef;
        private static ThoughtDef ateFineMealDef;
        private static ThoughtDef ateLavishMealDef;
        private static ThoughtDef impressiveDiningDef;

        private static PawnRelationDef[] closeRelations;

        /// <summary>
        /// Gifts and Birthdays' own congratulation memory, or null when that mod is absent. Looked
        /// up by def name rather than by packageId on purpose: a repack, a fork or a rename of that
        /// mod would change its packageId but not the def its party already writes on the pawn.
        /// </summary>
        public static ThoughtDef GiftsAndBirthdaysWishDef
        {
            get
            {
                if (!interopResolved)
                {
                    interopResolved = true;
                    giftsAndBirthdaysWishDef =
                        DefDatabase<ThoughtDef>.GetNamedSilentFail("BirthdayCongratulationReceived");
                }

                return giftsAndBirthdaysWishDef;
            }
        }

        private static void ResolveVanilla()
        {
            if (vanillaResolved)
            {
                return;
            }

            vanillaResolved = true;
            attendedPartyDef = DefDatabase<ThoughtDef>.GetNamedSilentFail("AttendedParty");
            ateFineMealDef = DefDatabase<ThoughtDef>.GetNamedSilentFail("AteFineMeal");
            ateLavishMealDef = DefDatabase<ThoughtDef>.GetNamedSilentFail("AteLavishMeal");
            impressiveDiningDef = DefDatabase<ThoughtDef>.GetNamedSilentFail("AteInImpressiveDiningRoom");
        }

        private static PawnRelationDef[] CloseRelations
        {
            get
            {
                if (closeRelations == null)
                {
                    closeRelations = new[]
                    {
                        PawnRelationDefOf.Spouse,
                        PawnRelationDefOf.Fiance,
                        PawnRelationDefOf.Lover,
                        PawnRelationDefOf.Parent,
                        PawnRelationDefOf.Child,
                        PawnRelationDefOf.Sibling
                    };
                }

                return closeRelations;
            }
        }

        /// <summary>
        /// True on the calendar anniversary of the pawn's birth. Deliberately not the biological
        /// birthday: Pawn_AgeTracker fires that one when the biological year rolls over, which
        /// drifts away from the calendar as soon as the aging rate is not exactly one (difficulty
        /// settings, genes, a growth vat). BirthDayOfYear is computed at longitude 0, so the
        /// current day is read at longitude 0 too and every map agrees on what day it is.
        /// </summary>
        public static bool IsBirthdayToday(Pawn pawn)
        {
            if (pawn?.ageTracker == null || pawn.Dead)
            {
                return false;
            }

            return GenDate.DayOfYear(Find.TickManager.TicksAbs, 0f) == pawn.ageTracker.BirthDayOfYear;
        }

        /// <summary>A pawn old enough and present enough to have a birthday worth noticing.</summary>
        public static bool CanCelebrate(Pawn pawn)
        {
            return pawn != null
                   && !pawn.Dead
                   && pawn.IsFreeColonist
                   && pawn.RaceProps != null
                   && pawn.RaceProps.Humanlike
                   && pawn.needs?.mood?.thoughts?.memories != null
                   && !pawn.DevelopmentalStage.Baby()
                   && pawn.ageTracker != null
                   && pawn.ageTracker.AgeBiologicalYears > 0;
        }

        /// <summary>
        /// True when <paramref name="wisher"/> has already wished <paramref name="celebrant"/> well
        /// today, through this mod or through Gifts and Birthdays. Keeps the interaction to one per
        /// pair and per birthday instead of letting it repeat every time they cross paths.
        /// </summary>
        public static bool AlreadyWished(Pawn celebrant, Pawn wisher)
        {
            List<Thought_Memory> memories = celebrant?.needs?.mood?.thoughts?.memories?.Memories;
            if (memories == null)
            {
                return false;
            }

            ThoughtDef other = GiftsAndBirthdaysWishDef;
            for (int i = 0; i < memories.Count; i++)
            {
                Thought_Memory memory = memories[i];
                if (memory.otherPawn != wisher || memory.age > TodayWindowTicks)
                {
                    continue;
                }

                if (memory.def == MHRDefOf.Nelim_BirthdayWishReceived || (other != null && memory.def == other))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Counts the distinct colonists who wished <paramref name="celebrant"/> a happy birthday
        /// today, and reports whether one of them was someone close. Congratulations handed out at
        /// a Gifts and Birthdays party count here, which is the whole point of reading memories
        /// rather than intercepting this mod's own interaction.
        /// </summary>
        public static int CountWishers(Pawn celebrant, out bool closeOneWished)
        {
            closeOneWished = false;

            List<Thought_Memory> memories = celebrant?.needs?.mood?.thoughts?.memories?.Memories;
            if (memories == null)
            {
                return 0;
            }

            ThoughtDef other = GiftsAndBirthdaysWishDef;
            List<Pawn> counted = new List<Pawn>();

            for (int i = 0; i < memories.Count; i++)
            {
                Thought_Memory memory = memories[i];
                if (memory.otherPawn == null || memory.otherPawn == celebrant || memory.age > TodayWindowTicks)
                {
                    continue;
                }

                if (memory.def != MHRDefOf.Nelim_BirthdayWishReceived && (other == null || memory.def != other))
                {
                    continue;
                }

                if (counted.Contains(memory.otherPawn))
                {
                    continue;
                }

                counted.Add(memory.otherPawn);
                if (IsCloseTo(celebrant, memory.otherPawn))
                {
                    closeOneWished = true;
                }
            }

            return counted.Count;
        }

        /// <summary>Family, a partner, or a friend the celebrant genuinely likes.</summary>
        public static bool IsCloseTo(Pawn celebrant, Pawn other)
        {
            if (celebrant?.relations == null || other == null)
            {
                return false;
            }

            PawnRelationDef[] relations = CloseRelations;
            for (int i = 0; i < relations.Length; i++)
            {
                if (relations[i] != null && celebrant.relations.DirectRelationExists(relations[i], other))
                {
                    return true;
                }
            }

            return celebrant.relations.OpinionOf(other) >= CloseFriendOpinion;
        }

        /// <summary>
        /// How the day went, on top of the wishes: a gathering, a meal worth the name, and a room
        /// worth eating it in. All three are vanilla memories the pawn formed by itself, so a Gifts
        /// and Birthdays party is recognised as a party without this mod knowing that mod exists.
        /// </summary>
        public static int DayQualityBonus(Pawn celebrant)
        {
            ResolveVanilla();

            int bonus = 0;
            if (HasMemoryToday(celebrant, attendedPartyDef))
            {
                bonus += 2;
            }

            if (HasMemoryToday(celebrant, ateLavishMealDef))
            {
                bonus += 2;
            }
            else if (HasMemoryToday(celebrant, ateFineMealDef))
            {
                bonus += 1;
            }

            if (HasMemoryToday(celebrant, impressiveDiningDef))
            {
                bonus += 1;
            }

            return bonus;
        }

        private static bool HasMemoryToday(Pawn pawn, ThoughtDef def)
        {
            if (def == null)
            {
                return false;
            }

            List<Thought_Memory> memories = pawn?.needs?.mood?.thoughts?.memories?.Memories;
            if (memories == null)
            {
                return false;
            }

            for (int i = 0; i < memories.Count; i++)
            {
                if (memories[i].def == def && memories[i].age <= TodayWindowTicks)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Turns the day's score into the stage of Nelim_BirthdayRemembered.</summary>
        public static int StageForScore(int score)
        {
            if (score >= 9)
            {
                return 4;
            }

            if (score >= 6)
            {
                return 3;
            }

            if (score >= 4)
            {
                return 2;
            }

            if (score >= 2)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>Wishes are worth more the more of them there are, with a ceiling.</summary>
        public static int PointsForWishers(int wishers)
        {
            if (wishers >= 5)
            {
                return 4;
            }

            if (wishers >= 3)
            {
                return 3;
            }

            return wishers;
        }
    }
}
