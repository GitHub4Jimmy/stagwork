using System;
using System.Collections.Generic;
using System.IO;
using TrainingFund.DNN.Integration.Domain;
using TrainingFund.Shared.Constants;

namespace TrainingFund.DNN.Integration.Helpers
{
    public static class DummyContentSettingsHelper
    {
        public const String settingsFile = "App_Data\\globalSettings.json";
        
        public const String ENVIRONMENT_SETTINGS_FILE = "TRAINING_FUND_SETTINGS_FILE";

        private static DateTime lastModified;
        private static DummyContentSettings lastSettings;

        public static DummyContentSettings GetSettings()
        {
            String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Environment.GetEnvironmentVariable(ENVIRONMENT_SETTINGS_FILE) ?? settingsFile);
            if (File.Exists(filePath))
            {
                DateTime modified = File.GetLastWriteTimeUtc(filePath);

                if (DateTime.Compare(lastModified, modified) != 0)
                {
                    String contents = File.ReadAllText(filePath);

                    var settings = StagwellTech.SEIU.CommonEntities.SerializableToJSON<DummyContentSettings>.fromJSON(contents);

                    lastSettings = settings;
                    lastModified = modified;
                }
            }

            return lastSettings;
        }

    }
}
