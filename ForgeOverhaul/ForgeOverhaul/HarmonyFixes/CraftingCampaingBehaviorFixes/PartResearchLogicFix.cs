using ForgeOverhaul.HarmonyFixes.CraftingFixes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace ForgeOverhaul.HarmonyFixes.CraftingCampaingBehaviorFixes
{
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "AddRPartResearch")]
    class PartResearchLogicFix
    {
        private static bool Prefix(CraftingCampaignBehavior __instance, int researchPoints)
        {
			var traverseOpenNewPartXP = Traverse.Create(__instance).Field("_openNewPartXP");
			var OpenedParts = ((List<CraftingPiece>)Traverse.Create(__instance).Field("_openedParts").GetValue());

			traverseOpenNewPartXP.SetValue((int)traverseOpenNewPartXP.GetValue() + researchPoints);
			
			if (OpenedParts != null)
			{
				var openedPartCount = (from x in OpenedParts where RememberLastDesignFix.LastWeaponCrafted.WeaponDesign.Template.Pieces.Contains(x) select x).Count<CraftingPiece>();
				var researchPointsForNewPart = Campaign.Current.Models.SmithingModel.ResearchPointsNeedForNewPart(openedPartCount);
				var newPartXP = (int)traverseOpenNewPartXP.GetValue();
				while (newPartXP > researchPointsForNewPart)
				{
					newPartXP -= researchPointsForNewPart;
					Traverse.Create(__instance).Method("OpenNewPart").GetValue();
					researchPointsForNewPart = Campaign.Current.Models.SmithingModel.ResearchPointsNeedForNewPart(++openedPartCount);
				}
				traverseOpenNewPartXP.SetValue(newPartXP);
			}
			return false;
        }
    }
}
