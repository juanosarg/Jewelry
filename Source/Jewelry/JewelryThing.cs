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
            //This will pick a random gem on creation, but crafted items should have it overridden with the recipe postfix
            gemstone ??= JewelryUtility.GetRandomGemstone();
        }

        public override IEnumerable<Thing> SmeltProducts(float efficiency)
        {
            if (!JewelryUtility.recipes.ContainsKey(this.def))
            {
                Log.Error($"Recipe not found for {this.def.defName}");
                IEnumerable<Thing> list = base.SmeltProducts(efficiency);
                foreach (Thing product in list)
                {
                    yield return product;
                }
            }
            RecipeDef recipe = JewelryUtility.recipes[this.def];
            int metalCost = recipe.ingredients.First(x => x.filter.AllowedThingDefs.Contains(Stuff)).CountRequiredOfFor(Stuff, recipe);
            int metalReturned = GenMath.RoundRandom(metalCost * 0.25f);
            if (metalReturned > 0)
            {
                Thing metal = ThingMaker.MakeThing(Stuff);
                metal.stackCount = metalReturned;
                yield return metal;
            }

            int gemCost = recipe.ingredients.First(x => x.filter.AllowedThingDefs.Contains(gemstone)).CountRequiredOfFor(gemstone, recipe);
            int gemReturned = GenMath.RoundRandom(gemCost * 0.25f);
            if (gemReturned > 0)
            {
                Thing gem = ThingMaker.MakeThing(gemstone);
                gem.stackCount = gemReturned;
                yield return gem;
            }
        }

        public override string Label => base.Label.Replace(Stuff.LabelAsStuff, $"{Stuff.LabelAsStuff} {gemstone.label}");
    }
}
