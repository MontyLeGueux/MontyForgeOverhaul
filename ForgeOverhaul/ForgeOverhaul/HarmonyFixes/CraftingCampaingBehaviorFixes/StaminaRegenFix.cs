using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace ForgeOverhaul.HarmonyFixes.CraftingCampaingBehaviorFixes
{
    public class HeroCraftingRecord
    {
        public HeroCraftingRecord()
        {
            this.CraftingStamina = 100;
        }

        [SaveableField(10)]
        public int CraftingStamina;
    }
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "HourlyTick")]
    public class CraftPatch
    {
        public static void Prefix(CraftingCampaignBehavior __instance)
        {
            Type HeroCraftingRecords = Traverse.Create<CraftingCampaignBehavior>().Type("HeroCraftingRecords").GetType();
            var heroCraftingRecords = Traverse.Create(__instance).Field("_heroCraftingRecords").GetValue();
            var enumerator = Traverse.Create(heroCraftingRecords).Method("GetEnumerator").GetValue();
            var enumNext = Traverse.Create(enumerator).Method("MoveNext");
            var enumGet = Traverse.Create(enumerator).Property("Current");
            while (true)
            {
                if (!(enumNext.GetValue<bool>()))
                {
                    break;
                }
                var current = enumGet.GetValue();
                var currentValue = Traverse.Create(current).Property("Value").GetValue();
                var craftingStamina = Traverse.Create(currentValue).Field<int>("CraftingStamina");
                var hero = Traverse.Create(current).Property<Hero>("Key").Value;
                if (craftingStamina.Value < __instance.GetMaxHeroCraftingStamina(hero))
                {
                    MobileParty partyBelongedTo = hero.PartyBelongedTo;
                    if (((partyBelongedTo != null) ? partyBelongedTo.CurrentSettlement : null) != null)
                    {
                        craftingStamina.Value = Math.Min(__instance.GetMaxHeroCraftingStamina(hero), craftingStamina.Value + ((__instance.GetMaxHeroCraftingStamina(hero) / 100) * 10));
                    }
                    else
                    {
                        craftingStamina.Value = Math.Min(__instance.GetMaxHeroCraftingStamina(hero), craftingStamina.Value + ((__instance.GetMaxHeroCraftingStamina(hero) / 100) * 5));
                    }
                    if (craftingStamina.Value == __instance.GetMaxHeroCraftingStamina(hero))
                    {
                        var heroCharacter = Campaign.Current.Characters.First(x => x.IsHero && x.HeroObject.StringId == hero.StringId);
                        InformationManager.DisplayMessage(new InformationMessage($"{(heroCharacter != null ? heroCharacter.Name : hero.GetName())}'s smithy stamina is full"));
                    }
                }
            }
        }
    }
}
