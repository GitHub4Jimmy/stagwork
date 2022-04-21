using StagwellTech.SEIU.CommonEntities.Utils;
using StagwellTech.SirenSDK.Models;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public static class MetricsLogger
    {
        public static void Log(string name, long time)
        {
            SirenFactory.SirenDNN.Provider.LogMetric(
                new Metric(name, time)
            );
        }
    }
}
