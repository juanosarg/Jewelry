using HarmonyLib;
using RimWorld;
using Verse;

namespace Jewelry
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmonyInstance = new Harmony("Kikohi.Jewelry");
            harmonyInstance.PatchAll();
        }
    }

    [HarmonyPatch(typeof(ApparelUtility), "CanWearTogether")]
    public static class ApparelUtility_CanWearTogether_PostFix
    {
        [HarmonyPostfix]
        public static void Postfix(ThingDef A, ThingDef B, ref bool __result)
        {
            if (A.defName.Contains("Jewelry_Necklace") && B.defName.Contains("Jewelry_Necklace"))
                __result = false;
            else if (A.defName.Contains("Jewelry_Bracelet") && B.defName.Contains("Jewelry_Bracelet"))
                __result = false;
            else if (A.defName.Contains("Jewelry_Earring") && B.defName.Contains("Jewelry_Earring"))
                __result = false;
            else if (A.defName.Contains("Jewelry_Ring") && B.defName.Contains("Jewelry_Ring"))
                __result = false;
        }
    }
}
