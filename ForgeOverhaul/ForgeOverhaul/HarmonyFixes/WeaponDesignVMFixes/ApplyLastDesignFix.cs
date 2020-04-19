using ForgeOverhaul.HarmonyFixes.CraftingFixes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection.Craft.WeaponDesign;
using TaleWorlds.Core;

namespace ForgeOverhaul.HarmonyFixes.WeaponDesignVMFixes
{
    [HarmonyPatch(typeof(WeaponDesignVM), "SetDefaultWeaponDesign")]
    class ApplyLastDesignFix
    {
        private static bool Prefix(WeaponDesignVM __instance)
        {
            var crafting = ((Crafting)Traverse.Create(__instance).Field("_crafting").GetValue());
            if (RememberLastDesignFix.Designs.ContainsKey(crafting.CurrentCraftingTemplate.TemplateName.ToString()))
            {
                var designToApply = RememberLastDesignFix.Designs[crafting.CurrentCraftingTemplate.TemplateName.ToString()];
                for (int i = 0; i < 4; i++)
                {
                    crafting.SwitchToPiece(designToApply.UsedPieces[i]);
                }
                return false;
            }
            return true;
        }
    }
}
