using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Jewelry
{
    public class JewelryThing : Apparel
    {
        public ThingDef gemstone;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref gemstone, "gemstone");
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (gemstone == null)
                {
                    if (JewelryUtility.GemstoneList.Contains(Stuff))
                    {
                        gemstone = Stuff;
                        SetStuffDirect(Rand.Value < 0.5f ? ThingDefOf.Gold : ThingDefOf.Silver);
                    }
                    else
                    {
                        gemstone = JewelryUtility.GetRandomGemstone();
                    }
                }
            }
        }

        //I've set the stuff to be the metal, so color one will already be correct
        public override Color DrawColorTwo
        {
            get
            {
                return gemstone.stuffProps.color;
            }
        }

        public override void PostMake()
        {
            base.PostMake();
            //This will pick a random gem on creation. Crafted items should have it overridden with the recipe postfix, so this will only affect jewelry that's not crafted
            gemstone ??= JewelryUtility.GetRandomGemstone();
        }

        //Can return both metal and gemstone when smelted
        public override IEnumerable<Thing> SmeltProducts(float efficiency)
        {
            if (!JewelryUtility.recipes.ContainsKey(def))
            {
                Log.Error($"Recipe not found for {def.defName}");
                IEnumerable<Thing> list = base.SmeltProducts(efficiency);
                foreach (Thing product in list)
                {
                    yield return product;
                }
            }

            RecipeDef recipe = JewelryUtility.recipes[def];
            if (SmeltResult(recipe, Stuff, out Thing metal))
            {
                yield return metal;
            }
            if (SmeltResult(recipe, gemstone, out Thing gem))
            {
                yield return gem;
            }
        }

        private bool SmeltResult(RecipeDef recipe, ThingDef ingredient, out Thing result)
        {
            int cost = recipe.ingredients.First(x => x.filter.AllowedThingDefs.Contains(ingredient)).CountRequiredOfFor(ingredient, recipe);
            int returned = GenMath.RoundRandom(cost * 0.25f);
            if (returned > 0)
            {
                result = ThingMaker.MakeThing(ingredient);
                result.stackCount = returned;
                return true;
            }
            result = null;
            return false;
        }

        //Add gemstone to the label
        public override string Label => base.Label.Replace(Stuff.LabelAsStuff, $"{Stuff.LabelAsStuff} {gemstone.label}");

        //Add gemstone to the info card
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            foreach (StatDrawEntry item in base.SpecialDisplayStats())
            {
                yield return item;
            }
            yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "StatGemstone_Name".Translate(), gemstone.LabelCap, "StatGemstone_Desc".Translate(), 1099, hyperlinks:
            [
                new Dialog_InfoCard.Hyperlink(gemstone)
            ]);
        }
    }
}
