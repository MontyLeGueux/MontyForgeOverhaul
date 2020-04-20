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
    class CoalConfigurableModel : DefaultSmithingModel
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

        public override IEnumerable<Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith)
        {
            yield return new Crafting.RefiningFormula(CraftingMaterials.Wood, 2, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, 3, CraftingMaterials.IronOre, 0);
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker))
            {
                yield return new Crafting.RefiningFormula(CraftingMaterials.Wood, 3, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, 6, CraftingMaterials.IronOre, 0);
            }
            yield return new Crafting.RefiningFormula(CraftingMaterials.IronOre, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? 3 : 2, CraftingMaterials.IronOre, 0);
            yield return new Crafting.RefiningFormula(CraftingMaterials.Iron1, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron2, 1, CraftingMaterials.IronOre, 0);
            yield return new Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron3, 1, CraftingMaterials.Iron1, 1);
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker))
            {
                yield return new Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron4, 1, CraftingMaterials.Iron1, 1);
            }
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker2))
            {
                yield return new Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron5, 1, CraftingMaterials.Iron1, 1);
            }
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
            {
                yield return new Crafting.RefiningFormula(CraftingMaterials.Iron5, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron6, 1, CraftingMaterials.Iron1, 1);
            }
            yield break;
        }
    }
}
