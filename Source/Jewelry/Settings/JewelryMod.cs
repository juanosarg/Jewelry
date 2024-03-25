using UnityEngine;
using Verse;

namespace Jewelry
{
    internal class JewelryMod : Mod
    {
        public static JewelrySettings settings;

        public JewelryMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<JewelrySettings>();
        }

        public override string SettingsCategory()
        {
            return "JewelrySettingsCategoryLabel".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard lst = new Listing_Standard();
            lst.Begin(inRect);
            lst.Gap();
            lst.CheckboxLabeled("shouldJewelryBoostMood".Translate() + ": ", ref JewelrySettings.jewelryBoostMood, null);

            if (ModLister.HasActiveModWithName("Royalty"))
            {
                lst.Gap();
                lst.Label("explanationRToggle".Translate());
                lst.Gap(8);
                lst.CheckboxLabeled("shouldJewelryNeed".Translate() + ": ", ref JewelrySettings.royalRequireJewelry, null);
            }
            lst.End();
            settings.Write();

            WorldComp_Requirements.UpdateRequirement();
        }
    }
}