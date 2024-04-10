using RimWorld;

namespace Jewelry
{
    [DefOf]
    public static class JewelryDefOf
    {
        public static StuffCategoryDef Gemstones;

        static JewelryDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(JewelryDefOf));
        }
    }
}
