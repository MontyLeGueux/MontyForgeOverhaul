using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using System.Linq;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using HarmonyLib;
using ForgeOverhaul.Config;
using ForgeOverhaul.SmithingModels;

namespace ForgeOverhaul
{
    public class ForgeOverhaulSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            ForgeOverhaulConfig.initConfig();
            Harmony harmony = new Harmony("MontyForgeOverhaul");
            harmony.PatchAll(typeof(ForgeOverhaulSubModule).Assembly);
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            InformationManager.DisplayMessage(new InformationMessage("Loaded Monty's Forge Overhaul"));

            if (!ForgeOverhaulConfig.ConfigLoadedSuccessfully)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Unable to read Config ({ForgeOverhaulConfig.ConfigLoadError}) - defaulting settings"));
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (!(game.GameType is Campaign))
            {
                return;
            }
            AddModels(gameStarterObject);
        }

        protected virtual void AddModels(IGameStarter gameStarterObject)
        {
            if (ForgeOverhaulConfig.ConfigSettings.ExtraCoalEnabled)
            {
                if (ForgeOverhaulConfig.ConfigSettings.SmithingStaminaEnabled)
                {
                    ReplaceModel<DefaultSmithingModel, CoalStaminaModel>(gameStarterObject);
                }
                else
                {
                    ReplaceModel<DefaultSmithingModel, CoalNoStaminaModel>(gameStarterObject);
                }
            }
            else
            {
                if (!ForgeOverhaulConfig.ConfigSettings.SmithingStaminaEnabled)
                {
                    ReplaceModel<DefaultSmithingModel, NoCoalNoStaminaModel>(gameStarterObject);
                }
            }
        }

        protected void ReplaceModel<TBaseType, TChildType>(IGameStarter gameStarterObject)
            where TBaseType : GameModel
            where TChildType : TBaseType
        {
            if (!(gameStarterObject.Models is IList<GameModel> models))
            {
                return;
            }

            bool found = false;
            for (int index = 0; index < models.Count; ++index)
            {
                if (models[index] is TBaseType)
                {
                    found = true;
                    if (models[index] is TChildType)
                    {
                    }
                    else
                    {
                        models[index] = Activator.CreateInstance<TChildType>();
                    }
                }
            }

            if (!found)
            {
                gameStarterObject.AddModel(Activator.CreateInstance<TChildType>());
            }
        }
    }
}
