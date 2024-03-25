using AchievementsExpanded;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace KustomTrackers
{
    [StaticConstructorOnStartup]
    class KustomTrackersHarmony
    {
		public static void AnyOfThingSpawned(Pawn worker, List<Thing> things)
		{
			foreach (var card in AchievementPointManager.GetCards<ItemCraftAnyOfTracker>())
			{
				try
				{
					foreach (Thing thing in things)
					{
						if (card.tracker is ItemCraftAnyOfTracker tracker && tracker != null)
						{
							if (tracker.Trigger(thing))
								card.UnlockCard();
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error($"Unable to trigger event for card validation. To avoid further errors {card.def.LabelCap} has been automatically unlocked.\n\nException={ex.Message}");
					card.UnlockCard();
				}
			}
		}

		public static void ApparelWeared(Pawn_ApparelTracker __instance)
		{
			foreach (var card in AchievementPointManager.GetCards<ApparelTracker>())
			{
				try
				{
					if (card.tracker is ApparelTracker tracker && tracker != null)
					{
						if (tracker.Trigger(__instance))
							card.UnlockCard();
					}
				}
				catch (Exception ex)
				{
					Log.Error($"Unable to trigger event for card validation. To avoid further errors {card.def.LabelCap} has been automatically unlocked.\n\nException={ex.Message}");
					card.UnlockCard();
				}
			}
		}
	}
}
