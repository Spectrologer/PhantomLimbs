using RimWorld;
using Verse;

namespace PhantomLimbs
{
    public class HediffComp_PsychicFocusDrain : HediffComp
    {
        // Configurable property for the amount of psyfocus to drain per second.
        public HediffCompProperties_PsychicFocusDrain Props => (HediffCompProperties_PsychicFocusDrain)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            // Only check on a tick interval to save performance. 2000 ticks = roughly 33 seconds in-game.
            if (Pawn.IsHashIntervalTick(2000))
            {
                var psyfocus = Pawn.psychicEntropy;
                if (psyfocus == null)
                {
                    // If the pawn somehow loses their psylink, remove the phantom limb.
                    Pawn.health.RemoveHediff(parent);
                    return;
                }

                // Drain the psyfocus. The value is multiplied by the interval to get a per-second rate.
                psyfocus.OffsetPsyfocusDirectly(-Props.psyfocusDrainPerSecond * (2000f / GenTicks.TicksPerRealSecond));

                // If psyfocus is completely drained, the phantom limb dissipates.
                if (psyfocus.CurrentPsyfocus <= 0f)
                {
                    // You could add a thought or a letter here to notify the player.
                    Log.Message($"[PhantomLimbs] {Pawn.LabelShort}'s phantom limb dissipated due to lack of psyfocus.");
                    Pawn.health.RemoveHediff(parent);
                }
            }
        }
    }
}