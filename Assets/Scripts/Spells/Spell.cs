using UnityEngine;

namespace Spells
{
    public abstract class Spell : MonoBehaviour
    {
        [SerializeReference]
        public SpellTypeInput[] spellTypeInput;

        public abstract void ChangeState(int inputIndex);
        public abstract void FailCasting();
    }

    [System.Serializable]
    public abstract class SpellTypeInput
    {
    }

    [System.Serializable]
    public class RotationWheelSpeedInput : SpellTypeInput
    {
        public float durationForRotationWheel;
        public float requiredSpeed;
    }

    [System.Serializable]
    public class RhythmClickerInput : SpellTypeInput
    {
        public int beatsCount = 5;
        public float timeBetweenBeats = 1f;
    }

    [System.Serializable]
    public class HoldReleaseInput : SpellTypeInput
    {
        public float durationInSeconds = 2f;
        public float accuracyInSecondsRequired = 0.5f;
        public HoldReleaseInputType inputHoldReleaseInputType = HoldReleaseInputType.Hold;
        public enum HoldReleaseInputType
        {
            Hold, Release
        }

    }
}