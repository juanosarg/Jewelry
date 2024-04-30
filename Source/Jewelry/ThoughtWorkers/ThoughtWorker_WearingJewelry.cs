using RimWorld;
using UnityEngine;
using Verse;

namespace Jewelry
{
    internal class ThoughtWorker_WearingJewelry : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (JewelrySettings.jewelryBoostMood)
            {
                int numberOfJewelry = -1;

                foreach (Apparel apparel in p.apparel.WornApparel)
                {
                    //use if (apparels[i].def.thingClass == typeof(JewelryThing)) instead?
                    if (apparel.def.defName.Contains("Jewelry_"))
                    {
                        numberOfJewelry++;
                    }
                }

                if (numberOfJewelry >= 0)
                {
                    return ThoughtState.ActiveAtStage(Mathf.Min(numberOfJewelry, def.stages.Count));
                }
            }
            return ThoughtState.Inactive;
        }
    }
}
