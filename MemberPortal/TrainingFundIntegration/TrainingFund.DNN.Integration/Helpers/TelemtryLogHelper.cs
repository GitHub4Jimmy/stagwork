using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingFund.DNN.Integration.Helpers
{
    public class TelemtryLogHelper
    {

        public static void Log(object o, Exception e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var properties = new Dictionary<string, string> {{"ObjectType ", o.GetType().FullName}};

            telemetry.TrackException(e, properties);
        }
    }
}
