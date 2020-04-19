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
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "GetMaxHeroCraftingStamina")]
    public class MaxStaminaFix
    {
        private static void Postfix(ref int __result, Hero hero)
        {
            __result = 100 + hero.GetSkillValue(DefaultSkills.Crafting) + (hero.GetAttributeValue(CharacterAttributesEnum.Endurance) * 10);
        }
    }
}
