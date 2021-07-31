using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace RoyalArsenal
{
    class Verb_Cloak : Verb
    {
        private CompProperties_AbilityGiveHediff giveHediffProps;
        private float DURATION = 30.0f;
        protected override bool TryCastShot()
        {
            if (ReloadableCompSource != null && currentTarget != null)
            {
                giveHediffProps = new CompProperties_AbilityGiveHediff();
                giveHediffProps.hediffDef = HediffDef.Named("RA_CloakingField");
                giveHediffProps.applyToSelf = true;
                giveHediffProps.onlyApplyToSelf = true;
                giveHediffProps.replaceExisting = true;

                ApplyCloak(currentTarget.Pawn, ReloadableCompSource);

                ReloadableCompSource.UsedOnce();
            }
            return true;
        }

        private void ApplyCloak(Pawn target, CompReloadable comp)
        {
            if (target == null)
            {
                return;
            }
            if (giveHediffProps.replaceExisting)
            {
                Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(giveHediffProps.hediffDef);
                if (firstHediffOfDef != null)
                {
                    target.health.RemoveHediff(firstHediffOfDef);
                }
            }
            Hediff hediff = HediffMaker.MakeHediff(giveHediffProps.hediffDef, target, giveHediffProps.onlyBrain ? target.health.hediffSet.GetBrain() : null);
            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
            if (hediffComp_Disappears != null)
            {
                hediffComp_Disappears.ticksToDisappear = GenTicks.SecondsToTicks(DURATION);
            }
            HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
            if (hediffComp_Link != null)
            {
                hediffComp_Link.other = target;
                //hediffComp_Link.drawConnection = target == parent.pawn;
            }
            target.health.AddHediff(hediff);

            ThingWithComps parent = comp.parent;
            Pawn wearer = comp.Wearer;
            GenExplosion.DoExplosion(wearer.Position, wearer.Map, 2.9f, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 1f);
        }
    }
}
