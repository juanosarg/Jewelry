using AchievementsExpanded;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace KustomTrackers
{
    internal class ItemCraftAnyOfTracker : ItemCraftTracker
    {
        public List<ThingDef> defs;

        public override string Key => "ItemCraftAnyOfTracker";

        public override MethodInfo PatchMethod => AccessTools.Method(typeof(KustomTrackersHarmony), nameof(KustomTrackersHarmony.AnyOfThingSpawned));

        protected override string[] DebugText => new string[] { $"Def: {defs?.Count}", $"MadeFrom: {madeFrom?.defName ?? "Any"}", $"Quality: {quality}", $"Count: {count}", $"Current: {triggeredCount}" };

        public ItemCraftAnyOfTracker()
        {
        }

        public ItemCraftAnyOfTracker(ItemCraftAnyOfTracker reference) : base(reference)
        {
            defs = reference.defs;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ThingDef>(ref defs, "defs", LookMode.Def);
        }

        public override bool Trigger(Thing thing)
        {
            if (defs.Contains(thing.def) && (madeFrom is null || madeFrom == thing.Stuff))
            {
                if (quality is null || (thing.TryGetQuality(out var qc) && qc >= quality))
                {
                    triggeredCount++;
                }
            }
            return triggeredCount >= count;
        }
    }
}