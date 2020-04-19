using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Craft.Refinement;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using static TaleWorlds.Core.Crafting;

namespace ForgeOverhaul.HarmonyFixes.RefinementVMFixes
{
    [HarmonyPatch(typeof(RefinementVM), "ExecuteSelectedRefinement")]
    class FastRefinementHotkeyFix
    {
        private static bool Prefix(RefinementVM __instance, Hero currentCraftingHero)
        {
			if (__instance.CurrentSelectedAction != null)
			{
				ICraftingCampaignBehavior craftingBehavior = (ICraftingCampaignBehavior)Traverse.Create(__instance).Field("_craftingBehavior").GetValue();
				var formula = __instance.CurrentSelectedAction.RefineFormula;
				var itemRoster = MobileParty.MainParty.ItemRoster;
				if (craftingBehavior != null)
				{
					if (Input.IsKeyDown(InputKey.LeftShift))
					{
						if (Input.IsKeyDown(InputKey.LeftControl))
						{
							while (craftingBehavior.GetHeroCraftingStamina(currentCraftingHero) >= Campaign.Current.Models.SmithingModel.GetEnergyCostForRefining(ref formula, currentCraftingHero)
								&& HasEnoughCraftingMaterial(itemRoster, formula))
							{
								craftingBehavior.DoRefinement(currentCraftingHero, __instance.CurrentSelectedAction.RefineFormula);
							}
						}
						else
						{
							for (int i = 0; i < 10 
								&& craftingBehavior.GetHeroCraftingStamina(currentCraftingHero) >= Campaign.Current.Models.SmithingModel.GetEnergyCostForRefining(ref formula, currentCraftingHero)
								&& HasEnoughCraftingMaterial(itemRoster, formula) ; i++)
							{
								craftingBehavior.DoRefinement(currentCraftingHero, __instance.CurrentSelectedAction.RefineFormula);
							}
						}
					}
					else
					{
						craftingBehavior.DoRefinement(currentCraftingHero, __instance.CurrentSelectedAction.RefineFormula);
					}
				}
				__instance.RefreshRefinementActionsList(currentCraftingHero);
				RefinementActionItemVM currentSelectedAction = __instance.CurrentSelectedAction;
				if (currentSelectedAction != null && !currentSelectedAction.IsEnabled)
				{
					currentSelectedAction = null;
				}
			}
			return false;
        }

		private static bool HasEnoughCraftingMaterial(ItemRoster roster, RefiningFormula formula)
		{
			bool result = true;

			if (formula.Input1Count > 0)
			{
				result = result && roster.GetItemNumber(Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(formula.Input1)) >= formula.Input1Count;
			}
			if (formula.Input2Count > 0)
			{
				result = result && roster.GetItemNumber(Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(formula.Input2)) >= formula.Input2Count;
			}

			return result;
		}
    }
}
