using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Jewelry;

[StaticConstructorOnStartup]
public static class HarmonyInit
{
	static HarmonyInit()
	{
		Harmony val = new Harmony("Kikohi.Jewelry");
		val.PatchAll();
        MakeRecipeList();
	}

    private static void MakeRecipeList()
    {
        IEnumerable<RecipeDef> defs = DefDatabase<RecipeDef>.AllDefsListForReading.Where(x => x.workerClass == typeof(RecipeWorker_Jewelry));
        foreach (RecipeDef recipe in defs)
        {
            JewelryUtility.recipes.Add(recipe.ProducedThingDef, recipe);
        }
    }
}
