namespace SongsDatabase.Test.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;


    public static class TestUtilities
    {
        public static Dictionary<string, string> GetSettingsLookup()
        {
            FileInfo dataRoot = new FileInfo(AppContext.BaseDirectory);
            var appConfigFilePath = Path.Combine(dataRoot.DirectoryName, "..\\..\\..\\App.config");

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

            fileMap.ExeConfigFilename = appConfigFilePath;

            Configuration configuration =
                ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);


            AppSettingsSection appSettingSection =
                (AppSettingsSection)configuration.GetSection("appSettings");

            Dictionary<string, string> settingsLookup = new Dictionary<string, string>();

            foreach (var settingKey in appSettingSection.Settings.AllKeys)
            {
                var settingValue = appSettingSection.Settings[settingKey];
                var settingValueString = settingValue.Value;

                if (!settingsLookup.ContainsKey(settingKey))
                {
                    settingsLookup.Add(settingKey, settingValueString);
                }
                else
                {
                    settingsLookup[settingKey] = settingValueString;
                }
            }

            return settingsLookup;
        }
    }
}
