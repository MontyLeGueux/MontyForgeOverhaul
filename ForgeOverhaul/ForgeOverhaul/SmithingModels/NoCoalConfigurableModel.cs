using ForgeOverhaul.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Core;

namespace ForgeOverhaul.SmithingModels
{
    class NoCoalConfigurableModel : DefaultSmithingModel
    {
        public override int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero)
        {
            int num = ForgeOverhaulConfig.ConfigSettings.RefiningStaminaCost;
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalRefiner))
            {
                num = (num + 1) / 2;
            }
            return num;
        }

        public override int GetEnergyCostForSmithing(ItemObject item, Hero hero)
        {
            int num = (int)(ForgeOverhaulConfig.ConfigSettings.SmithingStaminaCost * ((int)item.Tier + 1));
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalSmith))
            {
                num = (num + 1) / 2;
            }
            return num;
        }

        public override int GetEnergyCostForSmelting(ItemObject item, Hero hero)
        {
            int num = ForgeOverhaulConfig.ConfigSettings.SmeltingStaminaCost;
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalSmelter))
            {
                num = (num + 1) / 2;
            }
            return num;
        }
    }
}
