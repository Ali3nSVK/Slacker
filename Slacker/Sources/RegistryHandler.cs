using Microsoft.Win32;
using System;

namespace Slacker.Sources
{
    public static class RegistryHandler
    {
        private static readonly string RegistryKey = @"SOFTWARE\SlackerSettings";

        public static void SaveSlackerSettings(SlackerSettings settings)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKey);

            key.SetValue("Defaults", settings.Defaults, RegistryValueKind.DWord);
            key.SetValue("TimeInterval", settings.TimeInterval);
            key.SetValue("KeyPressed", settings.KeyPressed);
            key.SetValue("FullKeyPress", settings.FullKeyPress, RegistryValueKind.DWord);

            key.Close();
        }

        public static SlackerSettings LoadSlackerSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey);
            SlackerSettings settings = new SlackerSettings();

            int? regLoader;

            if (key != null)
            {
                regLoader = key.GetValue("Defaults") as int?;
                settings.Defaults = regLoader.HasValue && regLoader.Value == 1;

                regLoader = key.GetValue("FullKeyPress") as int?;
                settings.FullKeyPress = regLoader.HasValue && regLoader.Value == 1;

                settings.TimeInterval = key.GetValue("TimeInterval") as int?;
                Enum.TryParse(key.GetValue("KeyPressed") as string, out settings.KeyPressed);
                
            }
            else settings.Defaults = true;

            return settings;
        }
    }
}
