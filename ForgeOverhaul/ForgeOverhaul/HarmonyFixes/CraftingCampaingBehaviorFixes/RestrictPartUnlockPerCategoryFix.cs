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
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "OpenNewPart")]
    class RestrictPartUnlockPerCategory
    {
        private static bool Prefix(CraftingCampaignBehavior __instance)
        {
			Traverse.Create(__instance).Method("EnsureParts").GetValue();
			var allParts = (CraftingPiece[])Traverse.Create(__instance).Field("_allCraftingParts").GetValue();
			var openedParts = (List<CraftingPiece>)Traverse.Create(__instance).Field("_openedParts").GetValue();
			int totalPartCount = allParts.Length;
			int unlockedPartCount = openedParts.Count;
			SmithingModel smithingModel = Campaign.Current.Models.SmithingModel;
			CraftingPiece[] array = (from x in allParts
									 where !openedParts.Contains(x) && RememberLastDesignFix.LastWeaponCrafted.WeaponDesign.Template.Pieces.Contains(x)
									 select x).ToArray<CraftingPiece>();
			if (array.Length != 0)
			{
				CraftingPiece craftingPiece = MBRandom.ChooseWeighted<CraftingPiece>(array, (CraftingPiece x) => smithingModel.GetProbabalityToOpenPart(x));
				if (craftingPiece != null)
				{
					Traverse.Create(__instance).Method("OpenPart", craftingPiece).GetValue();
				}
			}
			return false;
        }
    }
}
