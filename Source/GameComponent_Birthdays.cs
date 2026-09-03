using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ManyHappyReturns
{
    /// <summary>
    /// Keeps the list of colonists whose birthday it is today, announces it once in the morning,
    /// and closes the books at midnight. GameComponent subclasses are instantiated by the game on
    /// their own, so there is no def and nothing to register.
    /// </summary>
    public class GameComponent_Birthdays : GameComponent
    {
        /// <summary>One game hour. Catches recruits, guests turned colonists and returning caravans.</summary>
        private const int RescanIntervalTicks = GenDate.TicksPerHour;

        /// <summary>Past this local hour the letter is pointless, so it is dropped, not queued.</summary>
        private const int LatestLetterHour = 20;

        /// <summary>Below this, nobody could reasonably have remembered.</summary>
        private const int MinWitnessesForForgotten = 2;

        private int lastAbsDay = -1;
        private int nextRescanTick;
        private List<BirthdayRecord> celebratingToday = new List<BirthdayRecord>();

        public GameComponent_Birthdays(Game game)
        {
        }

        private static int CurrentAbsDay => Find.TickManager.TicksAbs / GenDate.TicksPerDay;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastAbsDay, "lastAbsDay", -1);
            Scribe_Collections.Look(ref celebratingToday, "celebratingToday", LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.PostLoadInit && celebratingToday == null)
            {
                celebratingToday = new List<BirthdayRecord>();
            }
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            if (celebratingToday == null)
            {
                celebratingToday = new List<BirthdayRecord>();
            }

            if (lastAbsDay < 0)
            {
                lastAbsDay = CurrentAbsDay;
            }

            nextRescanTick = 0;
        }

        public override void GameComponentTick()
        {
            int absDay = CurrentAbsDay;
            if (lastAbsDay < 0)
            {
                lastAbsDay = absDay;
            }
            else if (absDay != lastAbsDay)
            {
                // The day is over: this is when a birthday is judged, not while it is still running.
                CloseOutDay();
                lastAbsDay = absDay;
                nextRescanTick = 0;
            }

            int now = Find.TickManager.TicksGame;
            if (now >= nextRescanTick)
            {
                nextRescanTick = now + RescanIntervalTicks;
                Rescan();
            }
        }

        /// <summary>Dev-mode entry point: picks up a birthday moved onto today straight away.</summary>
        public void DebugRescanNow()
        {
            Rescan();
        }

        /// <summary>Dev-mode entry point: closes the books without waiting for midnight.</summary>
        public int DebugCloseOutNow()
        {
            int count = celebratingToday.Count;
            CloseOutDay();
            return count;
        }

        private void Rescan()
        {
            for (int i = celebratingToday.Count - 1; i >= 0; i--)
            {
                Pawn tracked = celebratingToday[i].pawn;
                if (tracked == null || tracked.Dead || !BirthdayUtility.IsBirthdayToday(tracked))
                {
                    celebratingToday.RemoveAt(i);
                }
            }

            List<Map> maps = Find.Maps;
            for (int m = 0; m < maps.Count; m++)
            {
                List<Pawn> colonists = maps[m].mapPawns.FreeColonistsSpawned;
                for (int i = 0; i < colonists.Count; i++)
                {
                    Pawn pawn = colonists[i];
                    if (!BirthdayUtility.CanCelebrate(pawn) || !BirthdayUtility.IsBirthdayToday(pawn))
                    {
                        continue;
                    }

                    if (IsTracked(pawn))
                    {
                        continue;
                    }

                    BirthdayRecord record = new BirthdayRecord { pawn = pawn };
                    celebratingToday.Add(record);
                    TrySendLetter(record);
                }
            }
        }

        private bool IsTracked(Pawn pawn)
        {
            for (int i = 0; i < celebratingToday.Count; i++)
            {
                if (celebratingToday[i].pawn == pawn)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The game sends no letter for an ordinary birthday: LetterBirthdayBiological only fires
        /// when the pawn gained age-related injuries, and growth moments are Biotech children. The
        /// player would otherwise have no way to know the day had come.
        /// </summary>
        private static void TrySendLetter(BirthdayRecord record)
        {
            if (record.letterSent || !ManyHappyReturnsMod.Settings.morningLetter)
            {
                return;
            }

            Pawn pawn = record.pawn;
            if (pawn == null || !pawn.Spawned)
            {
                return;
            }

            record.letterSent = true;
            if (GenLocalDate.HourOfDay(pawn) > LatestLetterHour)
            {
                return;
            }

            Find.LetterStack.ReceiveLetter(
                "ManyHappyReturns.BirthdayLetterLabel".Translate(pawn.Named("PAWN")),
                "ManyHappyReturns.BirthdayLetterText".Translate(
                    pawn.Named("PAWN"),
                    pawn.ageTracker.AgeBiologicalYears.Named("AGE")),
                LetterDefOf.NeutralEvent,
                pawn);
        }

        private void CloseOutDay()
        {
            for (int i = 0; i < celebratingToday.Count; i++)
            {
                Evaluate(celebratingToday[i].pawn);
            }

            celebratingToday.Clear();
        }

        private static void Evaluate(Pawn pawn)
        {
            if (!BirthdayUtility.CanCelebrate(pawn))
            {
                return;
            }

            // Away or unconscious: no verdict either way. Caravan pawns run no social interactions
            // at all, and a colonist who spent the day bleeding out was not being snubbed.
            if (!pawn.Spawned || pawn.Downed)
            {
                return;
            }

            MemoryThoughtHandler memories = pawn.needs.mood.thoughts.memories;
            int wishers = BirthdayUtility.CountWishers(pawn, out bool closeOneWished);

            if (wishers == 0)
            {
                TryGiveForgotten(pawn, memories);
                return;
            }

            int score = BirthdayUtility.PointsForWishers(wishers)
                        + (closeOneWished ? 2 : 0)
                        + BirthdayUtility.DayQualityBonus(pawn);

            memories.TryGainMemory(
                MakeMemory(MHRDefOf.Nelim_BirthdayRemembered, BirthdayUtility.StageForScore(score)));
        }

        private static void TryGiveForgotten(Pawn pawn, MemoryThoughtHandler memories)
        {
            if (!ManyHappyReturnsMod.Settings.forgottenThought)
            {
                return;
            }

            // A recruit from last week expects nothing from anyone yet.
            if (pawn.records == null
                || pawn.records.GetValue(RecordDefOf.TimeAsColonistOrColonyAnimal)
                   < BirthdayUtility.MinTicksAsColonistForForgotten)
            {
                return;
            }

            if (CountOtherColonistsOnMap(pawn) < MinWitnessesForForgotten)
            {
                return;
            }

            memories.TryGainMemory(MakeMemory(MHRDefOf.Nelim_BirthdayForgotten, 0));
        }

        private static int CountOtherColonistsOnMap(Pawn pawn)
        {
            Map map = pawn.Map;
            if (map == null)
            {
                return 0;
            }

            int count = 0;
            List<Pawn> colonists = map.mapPawns.FreeColonistsSpawned;
            for (int i = 0; i < colonists.Count; i++)
            {
                if (colonists[i] != pawn && !colonists[i].DevelopmentalStage.Baby())
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// The settings' mood scale is baked into the memory as it is formed, through the vanilla
        /// moodPowerFactor field, so moving the slider never rewrites memories already held.
        /// </summary>
        private static Thought_Memory MakeMemory(ThoughtDef def, int stage)
        {
            Thought_Memory memory = ThoughtMaker.MakeThought(def, stage);
            memory.moodPowerFactor = ManyHappyReturnsMod.Settings.ClampedMoodFactor;
            return memory;
        }
    }
}
