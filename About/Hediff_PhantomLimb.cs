using System;
using Verse;
using RimWorld;

namespace PhantomLimbs
{
    public class Hediff_PhantomLimb : Hediff_AddedPart
    {
        private HediffComp_PartEfficiency _efficiencyComp;

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            _efficiencyComp = this.TryGetComp<HediffComp_PartEfficiency>();
        }

        // Override severity to prevent removal while allowing efficiency control
        public override float Severity
        {
            get => base.Severity;
            set
            {
                // Always maintain a minimum severity to prevent removal
                base.Severity = Math.Max(value, 0.1f);
            }
        }

        // Custom property to get the effective part efficiency
        public float EffectivePartEfficiency
        {
            get
            {
                if (_efficiencyComp != null)
                {
                    return _efficiencyComp.GetEffectiveEfficiency();
                }
                return def.addedPartProps?.partEfficiency ?? 1f;
            }
        }

        // Override to use effective efficiency for the body part
        public override float PartEfficiency => EffectivePartEfficiency;

        public override bool ShouldRemove => false; // Prevent removal
    }
}
