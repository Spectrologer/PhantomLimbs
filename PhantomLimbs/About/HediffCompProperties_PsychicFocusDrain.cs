using Verse;

namespace PhantomLimbs
{
    public class HediffCompProperties_PsychicFocusDrain : HediffCompProperties
    {
        public float psyfocusDrainPerSecond = 0.005f; // Default value
        public HediffCompProperties_PsychicFocusDrain() => compClass = typeof(HediffComp_PsychicFocusDrain);
    }
}