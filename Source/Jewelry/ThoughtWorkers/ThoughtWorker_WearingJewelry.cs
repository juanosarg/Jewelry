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
                var apparels = p.apparel.WornApparel;

                for (int i = 0; i < apparels.Count; i++)
                {
                    if (apparels[i].def.defName.Contains("Jewelry_"))
                        numberOfJewelry++;
                }

                if (numberOfJewelry >= 0)
                    return ThoughtState.ActiveAtStage(Mathf.Min(numberOfJewelry, def.stages.Count));
            }
            return ThoughtState.Inactive;
        }
    }
}
