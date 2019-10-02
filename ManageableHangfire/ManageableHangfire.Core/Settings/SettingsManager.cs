using Newtonsoft.Json;
using System;
using System.IO;

namespace ManageableHangfire.Core.Settings
{
    public static class SettingsManager
    {
        public static SettingsModel CurrentSettings
        {
            get
            {
                return ReadSettings();
            }
        }

        private static SettingsModel ReadSettings()
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\HangfireSettings.json";
            if (!File.Exists(filePath))
            {
                throw new Exception($"Settings file not found at '{filePath}'.");
            }

            var model = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(filePath));
            return model;
        }
    }
}
