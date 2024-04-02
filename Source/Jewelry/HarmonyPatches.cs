using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Jewelry
{
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

    [HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
    public static class GenRecipe_MakeRecipeProducts
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> __result, RecipeDef recipeDef, List<Thing> ingredients)
        {
            if (recipeDef.workerClass == typeof(RecipeWorker_Jewelry))
            {
                ThingDef gem = ingredients.First(x => x.def.IsStuff && x.def.stuffProps.categories.Contains(JewelryDefOf.Gemstones)).def;
                ThingDef metal = ingredients.First(x => x.def.IsStuff && x.def.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic)).def;
                foreach (Thing ingredient in __result)
                {
                    if (ingredient is JewelryThing jewelry)
                    {
                        jewelry.gemstone = gem;
                        //It sometimes chooses the gemstone for the stuff, so make sure it's the metal
                        jewelry.SetStuffDirect(metal);
                        yield return jewelry;
                    }
                }
                yield break;
            }
            foreach (Thing thing in __result)
            {
                yield return thing;
            }
        }
    }
}
