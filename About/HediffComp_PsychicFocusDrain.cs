﻿using RimWorld;
using Verse;

namespace PhantomLimbs
{
    public class HediffComp_PsychicFocusDrain : HediffComp
    {
        private HediffComp_PartEfficiency _efficiencyComp;

        // Configurable property for the amount of psyfocus to drain per second.
        public HediffCompProperties_PsychicFocusDrain Props => (HediffCompProperties_PsychicFocusDrain)props;

        private const float MIN_PSYFOCUS_PERCENTAGE = 0.1f; // 10% minimum psyfocus required

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            // Cache the efficiency component for performance.
            _efficiencyComp ??= parent.TryGetComp<HediffComp_PartEfficiency>();
            if (_efficiencyComp == null)
            {
                // If the efficiency comp is missing, log an error and do nothing.
                Log.ErrorOnce("[PhantomLimbs] HediffComp_PsychicFocusDrain requires HediffComp_PartEfficiency but it was not found.", parent.def.GetHashCode() + 12345);
                return;
            }

            // Only check on a tick interval to save performance. 2000 ticks = roughly 33 seconds in-game.
            if (Pawn.IsHashIntervalTick(2000))
            {
                var psyfocus = Pawn.psychicEntropy;
                var psylink = Pawn.GetPsylinkLevel();

                // Check for conditions that would make the limb non-functional.
                bool hasPsylink = psylink >= Props.minPsylinkLevel;
                bool hasPsyfocus = psyfocus != null && psyfocus.CurrentPsyfocus >= MIN_PSYFOCUS_PERCENTAGE;

                if (!hasPsylink || !hasPsyfocus)
                {
                    // The limb is inactive. Set its efficiency to 0.
                    _efficiencyComp.SetEfficiency(false);
                    return;
                }

                // If we've reached this point, the limb should be active.
                _efficiencyComp.SetEfficiency(true);

                // Drain the psyfocus. The value is multiplied by the interval to get a per-second rate.
                psyfocus.OffsetPsyfocusDirectly(-Props.psyfocusDrainPerSecond * (2000f / GenTicks.TicksPerRealSecond));
            }
        }

        // Add dynamic description showing current status
        public override string CompDescriptionExtra
        {
            get
            {
                var psyfocus = Pawn.psychicEntropy;
                var psylink = Pawn.GetPsylinkLevel();

                string status = "";

                if (psylink < Props.minPsylinkLevel)
                {
                    status += $"\nRequires psylink level {Props.minPsylinkLevel} or higher. Current: {psylink}";
                }

                if (psyfocus == null)
                {
                    status += "\nNo psychic entropy tracker found.";
                }
                else
                {
                    float currentPsyfocus = psyfocus.CurrentPsyfocus;
                    status += $"\nRequires {MIN_PSYFOCUS_PERCENTAGE:P0} psyfocus. Current: {currentPsyfocus:P1}";
                    status += $"\nPsyfocus drain: {Props.psyfocusDrainPerSecond:F3} per second";
                }

                return status;
            }
        }
    }
}
