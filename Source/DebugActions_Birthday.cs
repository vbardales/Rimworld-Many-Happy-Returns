using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse;

namespace ManyHappyReturns
{
    /// <summary>
    /// Test helpers, visible only in dev mode.
    ///
    /// The game's own "Increment time" is useless here: it calls DebugSetTicksGame, which jumps the
    /// tick counter instead of playing the ticks. Nobody gets the chance to wish anybody anything,
    /// so a jumped-over birthday produces a forgotten one, or nothing at all. The way to test this
    /// mod is to move a birthday onto today and then let the game actually run.
    /// </summary>
    public static class DebugActions_Birthday
    {
        private const string Category = "Many Happy Returns";

        // Attribute names cannot be translated at runtime. Native yielded nodes can,
        // including their tool cursor labels, without patching the debug menu.
        [DebugActionYielder]
        private static IEnumerable<DebugActionNode> Actions()
        {
            yield return new DebugActionNode("ManyHappyReturns.Debug.Today".Translate(),
                DebugActionType.ToolMapForPawns, pawnAction: MakeBirthdayToday)
            {
                category = Category,
                labelGetter = () => "ManyHappyReturns.Debug.Today".Translate(),
                sourceAttribute = new DebugActionAttribute { allowedGameStates = AllowedGameStates.PlayingOnMap }
            };
            yield return new DebugActionNode("ManyHappyReturns.Debug.End".Translate(), action: EndBirthdaysNow)
            {
                category = Category,
                labelGetter = () => "ManyHappyReturns.Debug.End".Translate(),
                sourceAttribute = new DebugActionAttribute { allowedGameStates = AllowedGameStates.PlayingOnMap }
            };
            yield return new DebugActionNode("ManyHappyReturns.Debug.Tally".Translate(),
                DebugActionType.ToolMapForPawns, pawnAction: LogTally)
            {
                category = Category,
                labelGetter = () => "ManyHappyReturns.Debug.Tally".Translate(),
                sourceAttribute = new DebugActionAttribute { allowedGameStates = AllowedGameStates.PlayingOnMap }
            };
        }

        /// <summary>
        /// Moves the pawn's birth date so that today is their birthday, keeping their chronological
        /// age at a whole number of years. Biological age is stored separately and is untouched.
        /// </summary>
        private static void MakeBirthdayToday(Pawn p)
        {
            if (p?.ageTracker == null)
            {
                return;
            }

            int years = Mathf.Max(1, p.ageTracker.AgeChronologicalYears);
            p.ageTracker.BirthAbsTicks = Find.TickManager.TicksAbs - (long)years * GenDate.TicksPerYear;

            Current.Game?.GetComponent<GameComponent_Birthdays>()?.DebugRescanNow();

            Messages.Message(
                "ManyHappyReturns.Debug.Moved".Translate(p.Named("PAWN"), p.ageTracker.BirthDayOfYear.Named("DAY")),
                p, MessageTypeDefOf.NeutralEvent, historical: false);
        }

        /// <summary>
        /// Closes the books immediately instead of waiting for midnight, so the whole loop can be
        /// checked inside one game hour.
        /// </summary>
        private static void EndBirthdaysNow()
        {
            GameComponent_Birthdays component = Current.Game?.GetComponent<GameComponent_Birthdays>();
            if (component == null)
            {
                return;
            }

            int closed = component.DebugCloseOutNow();
            Messages.Message("ManyHappyReturns.Debug.Closed".Translate(closed.Named("COUNT")),
                MessageTypeDefOf.NeutralEvent, historical: false);
        }

        /// <summary>Prints the day's tally for one pawn, the way the verdict will read it.</summary>
        private static void LogTally(Pawn p)
        {
            if (p?.ageTracker == null)
            {
                return;
            }

            bool today = BirthdayUtility.IsBirthdayToday(p);
            int wishers = BirthdayUtility.CountWishers(p, out bool closeOne);
            int bonus = BirthdayUtility.DayQualityBonus(p);
            int score = BirthdayUtility.PointsForWishers(wishers) + (closeOne ? 2 : 0) + bonus;

            List<string> names = new List<string>();
            List<Thought_Memory> memories = p.needs?.mood?.thoughts?.memories?.Memories;
            ThoughtDef other = BirthdayUtility.GiftsAndBirthdaysWishDef;
            if (memories != null)
            {
                for (int i = 0; i < memories.Count; i++)
                {
                    Thought_Memory memory = memories[i];
                    if (memory.otherPawn == null || memory.age > BirthdayUtility.TodayWindowTicks)
                    {
                        continue;
                    }

                    if (memory.def == MHRDefOf.Nelim_BirthdayWishReceived || (other != null && memory.def == other))
                    {
                        names.Add($"{memory.otherPawn.LabelShort} ({memory.def.defName})");
                    }
                }
            }

            Log.Message(
                $"[Many Happy Returns] {p.LabelShortCap}\n"
                + $"  birthday today: {today} (born on day {p.ageTracker.BirthDayOfYear}, today is day {GenDate.DayOfYear(Find.TickManager.TicksAbs, 0f)})\n"
                + $"  wishers: {wishers} [{string.Join(", ", names)}]\n"
                + $"  close one among them: {closeOne}\n"
                + $"  party / meal / dining room bonus: {bonus}\n"
                + $"  score: {score} -> stage {BirthdayUtility.StageForScore(score)}\n"
                + $"  Gifts and Birthdays detected: {(other != null ? "yes" : "no")}");
        }
    }
}
