using UnityEngine;
using Verse;

namespace ManyHappyReturns
{
    public class ManyHappyReturnsSettings : ModSettings
    {
        public const float MinMoodFactor = 0.5f;
        public const float MaxMoodFactor = 2f;

        /// <summary>A letter on the morning of a colonist's birthday. The game sends none.</summary>
        public bool morningLetter = true;

        /// <summary>The negative memory formed when nobody said a word all day.</summary>
        public bool forgottenThought = true;

        /// <summary>
        /// Multiplies the mood of this mod's own memories, and nothing else. Applied through
        /// Thought_Memory.moodPowerFactor at the moment the memory is formed, so changing it
        /// mid-game leaves memories already formed as they were.
        /// </summary>
        public float moodFactor = 1f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref morningLetter, "morningLetter", true);
            Scribe_Values.Look(ref forgottenThought, "forgottenThought", true);
            Scribe_Values.Look(ref moodFactor, "moodFactor", 1f);
        }

        public void Reset()
        {
            morningLetter = true;
            forgottenThought = true;
            moodFactor = 1f;
        }

        public float ClampedMoodFactor => Mathf.Clamp(moodFactor, MinMoodFactor, MaxMoodFactor);
    }
}
