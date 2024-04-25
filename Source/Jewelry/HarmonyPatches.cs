using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Verse;

namespace Jewelry
{
    [HarmonyPatch(typeof(ApparelUtility), nameof(ApparelUtility.CanWearTogether))]
    public static class ApparelUtility_CanWearTogether_PostFix
    {
        [HarmonyPostfix]
        public static void Postfix(ThingDef A, ThingDef B, ref bool __result)
        {
            if (__result && A.thingClass == typeof(JewelryThing) && B.thingClass == typeof(JewelryThing))
            {
                //This will stop the psychic versions from being worn with the regular versions
                if (A.defName.Contains("Necklace") && B.defName.Contains("Necklace"))
                {
                    __result = false;
                }
                else if (A.defName.Contains("Bracelet") && B.defName.Contains("Bracelet"))
                {
                    __result = false;
                }
                else if (A.defName.Contains("Earring") && B.defName.Contains("Earring"))
                {
                    __result = false;
                }
                else if (A.defName.Contains("Ring") && B.defName.Contains("Ring"))
                {
                    __result = false;
                }
            }
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

    //Convert old silver jewelry to the unified defName
    [HarmonyPatch(typeof(BackCompatibilityConverter_Universal), nameof(BackCompatibilityConverter_Universal.BackCompatibleDefName))]
    public static class BackCompatibilityConverter_Universal_BackCompatibleDefName
    {
        public static bool Prefix(Type defType, string defName, ref string __result, ref bool __state)
        {
            if (defType == typeof(ThingDef) && defName.Contains("Jewelry_"))
            {
                switch (defName)
                {
                    case "Jewelry_Ring_Silver":
                        __result = "Jewelry_Ring";
                        break;
                    case "Jewelry_Necklace_Silver":
                        __result = "Jewelry_Necklace";
                        break;
                    case "Jewelry_Earring_Silver":
                        __result = "Jewelry_Earring";
                        break;
                    case "Jewelry_Bracelet_Silver":
                        __result = "Jewelry_Bracelet";
                        break;
                    default:
                        break;
                }
                if (__result != string.Empty)
                {
                    __state = true;
                    return false;
                }
            }
            return true;
        }
    }

    //Convert old jewelry with the Apparel thingClass to the new JewelryThing
    [HarmonyPatch(typeof(BackCompatibility), nameof(BackCompatibility.GetBackCompatibleType))]
    public static class BackCompatibility_GetBackCompatibleType
    {
        public static bool Prefix(string providedClassName, XmlNode node, ref Type __result)
        {
            if (providedClassName == "Apparel")
            {
                string defName = node["def"]?.InnerText;
                if (defName?.Contains("Jewelry_") ?? false)
                {
                    XmlElement stuff = node["stuff"];
                    XmlElement gemstone = node.OwnerDocument.CreateElement("gemstone");
                    gemstone.InnerText = stuff.InnerText;
                    node.AppendChild(gemstone);
                    if (defName.Contains("Silver"))
                    {
                        stuff.InnerText = "Silver";
                    }
                    else
                    {
                        stuff.InnerText = "Gold";
                    }
                    __result = typeof(JewelryThing);
                    return false;
                }
            }
            return true;
        }
    }
}
