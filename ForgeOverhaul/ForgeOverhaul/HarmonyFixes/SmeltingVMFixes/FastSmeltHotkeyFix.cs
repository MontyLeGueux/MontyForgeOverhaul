using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Craft.Smelting;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;

namespace ForgeOverhaul.HarmonyFixes.SmeltingVMFixes
{
	[HarmonyPatch(typeof(SmeltingVM), "SmeltSelectedItems")]
	class FastSmeltHotkeyFix
	{
		private static bool Prefix(SmeltingVM __instance, Hero currentCraftingHero)
		{
			if (Traverse.Create(__instance).Field("_currentSelectedItem").GetValue() != null
				&& Traverse.Create(__instance).Field("_smithingBehavior").GetValue() != null)
			{				ICraftingCampaignBehavior smithingBehavior = (ICraftingCampaignBehavior)Traverse.Create(__instance).Field("_smithingBehavior").GetValue();
				var itemRoster = MobileParty.MainParty.ItemRoster;
				if (smithingBehavior != null)
				{
					if (Input.IsKeyDown(InputKey.LeftShift))
					{
						var stackAmount = __instance.CurrentSelectedItem.NumOfItems;
						var charcoalAmount = itemRoster.GetItemNumber(DefaultItems.Charcoal);
						var energyCost = Campaign.Current.Models.SmithingModel.GetEnergyCostForSmelting(__instance.CurrentSelectedItem.Item, currentCraftingHero);
						var heroStamina = smithingBehavior.GetHeroCraftingStamina(currentCraftingHero);
						for (int i = 0; i < stackAmount
							&& heroStamina >= energyCost
							&& charcoalAmount >= 1 ; i++)
						{
							smithingBehavior.DoSmelting(currentCraftingHero, ((SmeltingItemVM)Traverse.Create(__instance).Field("_currentSelectedItem").GetValue()).Item);
							__instance.RefreshList();
							charcoalAmount--;
							heroStamina -= energyCost;
						}	
					}
					else
					{
						smithingBehavior.DoSmelting(currentCraftingHero, ((SmeltingItemVM)Traverse.Create(__instance).Field("_currentSelectedItem").GetValue()).Item);
					}
				}
			}
			__instance.RefreshList();
			if (__instance.CurrentSelectedItem != null)
			{
				SmeltingItemVM newItem = __instance.SmeltableItemList.FirstOrDefault((SmeltingItemVM i) => i.Item == __instance.CurrentSelectedItem.Item) ?? __instance.SmeltableItemList.FirstOrDefault<SmeltingItemVM>();
				Traverse.Create(__instance).Method("OnItemSelection", newItem).GetValue();
			}
			Traverse.Create(__instance).Method("_updateYieldValuesAcion").GetValue();
			return false;
		}
	}
}