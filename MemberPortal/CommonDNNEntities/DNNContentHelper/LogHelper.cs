using DotNetNuke.Entities.Modules;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonDNNEntities.DNNContentHelper
{
   public static class LogHelper
   {
        
        public static void Log(PortalModuleBase moduleContext,Exception message)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var properties = new Dictionary<string, string> { { "PageId ", moduleContext.TabId.ToString() } };
            properties.Add("ModuleId ", moduleContext.ModuleId.ToString());
            properties.Add("TabModuleId ", moduleContext.TabModuleId.ToString());
            properties.Add("UserId ", moduleContext.UserId.ToString());

            telemetry.TrackException(message, properties);
        }
        

   }
}
