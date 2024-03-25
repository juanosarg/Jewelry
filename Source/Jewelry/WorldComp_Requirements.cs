using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Jewelry
{
    internal class WorldComp_Requirements : WorldComponent
    {
        public WorldComp_Requirements(World world) : base(world) { }

        public static bool removed = false;

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            UpdateRequirement();
        }

        public static void UpdateRequirement()
        {
            if (!ModLister.RoyaltyInstalled)
                return;

            if (!removed && !JewelrySettings.royalRequireJewelry)
                RemoveRequirement();
        }

        private static void RemoveRequirement()
        {
            var defs = DefDatabase<RoyalTitleDef>.AllDefsListForReading;
            for (int i = 0; i < defs.Count; i++)
            {
                var title = defs[i];
                if (title.requiredApparel != null && title.requiredApparel.Any(r => r.requiredTags.Contains("Jewelry")))
                {
                    title.requiredApparel.RemoveAll(r => r.requiredTags.Contains("Jewelry"));
                }
            }
        }
    }
}