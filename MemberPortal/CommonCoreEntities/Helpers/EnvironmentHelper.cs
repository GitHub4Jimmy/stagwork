using System;

namespace StagwellTech.SEIU.CommonCoreEntities.Helpers
{
    public class EnvironmentHelper
    {
        public static int GetInt(string key, int defaultValue = 0)
        {
            var envValue = Environment.GetEnvironmentVariable(key);

            if (!String.IsNullOrEmpty(envValue)
                && Int32.TryParse(envValue, out int result))
            {
                return result;
            }

            return defaultValue;
        }


        public static bool GetBoolean(string key, bool defaultValue = false)
        {
            var envValue = Environment.GetEnvironmentVariable(key);

            if (!String.IsNullOrEmpty(envValue)
                && Boolean.TryParse(envValue, out bool result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}
