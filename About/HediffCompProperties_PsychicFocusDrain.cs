﻿using Verse;

namespace PhantomLimbs
{
    public class HediffCompProperties_PsychicFocusDrain : HediffCompProperties
    {
        public int minPsylinkLevel = 1; // Must be public to be set from XML
        public float psyfocusDrainPerSecond = 0.005f; // Must be public to be set from XML
        public HediffCompProperties_PsychicFocusDrain() => compClass = typeof(HediffComp_PsychicFocusDrain);
    }
}