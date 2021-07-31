using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace RoyalArsenal
{
    public class Verb_Chaff : Verb
    {
		protected override bool TryCastShot()
		{
			Pop(base.ReloadableCompSource);
			return true;
		}

		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return base.EquipmentSource.GetStatValue(StatDefOf.SmokepopBeltRadius);
		}

		public override void DrawHighlight(LocalTargetInfo target)
		{
			DrawHighlightFieldRadiusAroundTarget(caster);
		}

		public static void Pop(CompReloadable comp)
		{
			if (comp != null && comp.CanBeUsed)
			{
				ThingWithComps parent = comp.parent;
				Pawn wearer = comp.Wearer;
				GenExplosion.DoExplosion(wearer.Position, wearer.Map, parent.GetStatValue(StatDefOf.SmokepopBeltRadius), DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDef.Named("RA_Chaff"), 1f);
				comp.UsedOnce();
			}
		}
	}
}
