using Verse;

namespace PhantomLimbs
{
    public class HediffComp_PartEfficiency : HediffComp
    {
        private float? initialEfficiency;
        private bool isActive = true; // Default to active

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            // Store the initial efficiency when the hediff is first added.
            if (parent.def.addedPartProps != null)
            {
                initialEfficiency = parent.def.addedPartProps.partEfficiency;
            }
        }

        // A public method to allow other components to set the efficiency.
        public void SetEfficiency(bool active)
        {
            isActive = active;
        }

        // Override to provide dynamic efficiency based on active state
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            // Keep severity at 1.0 to prevent hediff removal, but efficiency will be controlled separately
            if (parent.Severity != 1f)
            {
                parent.Severity = 1f;
            }
        }

        // Custom method to get the actual efficiency based on active state
        public float GetEffectiveEfficiency()
        {
            if (!isActive)
            {
                return 0.0001f; // Inactive limbs have effectively 0 efficiency
            }
            return initialEfficiency ?? parent.def.addedPartProps?.partEfficiency ?? 1f;
        }
    }
}
