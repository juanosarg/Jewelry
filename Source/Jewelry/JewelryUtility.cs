using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Jewelry
{
    public static class JewelryUtility
    {
        public static readonly List<ThingDef> GemstoneList = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.IsStuff && x.stuffProps.categories.Contains(JewelryDefOf.Gemstones)).ToList();
        public static Dictionary<ThingDef, RecipeDef> recipes = new();
        public static ThingDef GetRandomGemstone() => GemstoneList.RandomElementByWeight(x => x.stuffProps.commonality);
    }
}
