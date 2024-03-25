using AchievementsExpanded;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace KustomTrackers
{
    public class ApparelTracker : Tracker<Pawn_ApparelTracker>
    {
        public List<ThingDef> defs;
        public ThingDef madeFrom;
        public QualityCategory? quality;

        public override string Key => "ItemCraftTracker";

        public override MethodInfo MethodHook => AccessTools.Method(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.Wear));
        public override MethodInfo PatchMethod => AccessTools.Method(typeof(KustomTrackersHarmony), nameof(KustomTrackersHarmony.ApparelWeared));

        protected override string[] DebugText => new string[] { $"Def: {defs?.Count}", $"MadeFrom: {madeFrom?.defName ?? "Any"}", $"Quality: {quality}" };

        public ApparelTracker()
        {
        }

        public ApparelTracker(ApparelTracker reference) : base(reference)
        {
            defs = reference.defs;
            madeFrom = reference.madeFrom;
            quality = reference.quality;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref defs, "defs", LookMode.Def);
            Scribe_Defs.Look(ref madeFrom, "madeFrom");
            Scribe_Values.Look(ref quality, "quality");
        }

        public override bool Trigger(Pawn_ApparelTracker at)
        {
            base.Trigger(at);
            foreach (ThingDef item in defs)
            {
                if (item.IsApparel && at.WornApparel.Find(a => a.def == item) is Apparel apparel && apparel != null && 
                    (madeFrom is null || madeFrom == apparel.Stuff) && (quality is null || (apparel.TryGetQuality(out var qc) && qc >= quality)))
                {
                    
                }
                else return false;
            }
            return true;
        }
    }
}