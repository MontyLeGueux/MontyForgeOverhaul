using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace ForgeOverhaul.HarmonyFixes.CraftingFixes
{
    [HarmonyPatch(typeof(Crafting), "SwitchToPiece")]
    public class RememberLastDesignFix
    {
        private static Dictionary<string, WeaponDesign> designs = new Dictionary<string, WeaponDesign>();
        private static ItemObject lastWeaponCrafted;

        public static Dictionary<string, WeaponDesign> Designs { get => designs;}
        public static ItemObject LastWeaponCrafted { get => lastWeaponCrafted;}

        private static void Postfix(Crafting __instance)
        {
            designs[__instance.CurrentCraftingTemplate.TemplateName.ToString()] = __instance.CurrentWeaponDesign;
            lastWeaponCrafted = __instance.GetCurrentCraftedItemObject();
        }
    }
}
