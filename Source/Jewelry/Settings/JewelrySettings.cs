using Verse;

namespace Jewelry
{
    public class JewelrySettings : ModSettings
    {
        public static bool jewelryBoostMood = true;
        public static bool royalRequireJewelry = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref jewelryBoostMood, "shouldJewelryBoostMood", true, false);
            Scribe_Values.Look(ref royalRequireJewelry, "shouldRoyalRequireJewelry", true, false);
        }
    }
}