using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities;

namespace TrainingFund.DNN.Integration.Helpers
{
    public static class DummyContentHelper
    {
        public static T Load<T>(string key)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();
            String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                globalSettings.DebugObjectPaths[key]);
            return SerializableToJSON<T>.fromJSONFile(filePath);
        }

        public static byte[] LoadBinary(string key)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();
            String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                globalSettings.DebugObjectPaths[key]);

            return File.ReadAllBytes(filePath);
        }

        public static void WriteToFile<T>(string key, T obj)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();
            String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                globalSettings.DebugObjectPaths[key]);

            var contents = SerializableToJSON<T>.toJSON(obj);

            File.WriteAllText(filePath, contents);
        }
    }
}
