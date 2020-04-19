using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOverhaul.Config
{
    public class ConfigSettings
    {
        [JsonProperty("SmithingStaminaEnabled")]
        public bool SmithingStaminaEnabled { get; set; }

        [JsonProperty("ExtraCoalEnabled")]
        public bool ExtraCoalEnabled { get; set; }
    }

    public static class ForgeOverhaulConfig
    {
        public static ConfigSettings ConfigSettings { get; set; }
        public static string ConfigLoadError { get; set; }

        private static readonly string ConfigFilePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MontyForgeOverhaul.json");

        private static readonly bool configExists = File.Exists(ConfigFilePath);

        public static bool ConfigLoadedSuccessfully { get; set; }

        public static void initConfig()
        {
            string error = "";
            if (configExists)
            {
                try
                {
                    ConfigSettings = JsonConvert.DeserializeObject<ConfigSettings>(File.ReadAllText(ConfigFilePath));
                    ConfigLoadedSuccessfully = true;
                }
                catch(Exception ex)
                {
                    error = ex.Message;
                }
            }
            if (ConfigSettings == null)
            {
                ConfigLoadedSuccessfully = false;
                ConfigSettings = new ConfigSettings();

                ConfigSettings.SmithingStaminaEnabled = true;
                ConfigSettings.ExtraCoalEnabled = true;
                if (error == null)
                {
                    error = "Config Not Found";
                }
            }
             ConfigLoadError = error;
        }
    }
}
