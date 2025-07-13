﻿using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Jewelry
{
    internal class WorldComp_Requirements : WorldComponent
    {
        public WorldComp_Requirements(World world) : base(world) { }

        public static bool removed = false;

        public override void FinalizeInit(bool fromLoad)
        {
            base.FinalizeInit(fromLoad);
            UpdateRequirement();
        }

        public static void UpdateRequirement()
        {
            if (!ModLister.RoyaltyInstalled)
            {
                return;
            }

            if (!removed && !JewelrySettings.royalRequireJewelry)
            {
                RemoveRequirement();
            }
        }

        private static void RemoveRequirement()
        {
            List<RoyalTitleDef> defs = DefDatabase<RoyalTitleDef>.AllDefsListForReading;
            for (int i = 0; i < defs.Count; i++)
            {
                RoyalTitleDef title = defs[i];
                if (title.requiredApparel != null && title.requiredApparel.Any(r => r.requiredTags.Contains("Jewelry")))
                {
                    title.requiredApparel.RemoveAll(r => r.requiredTags.Contains("Jewelry"));
                }
            }
        }
    }
}