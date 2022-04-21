using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public class BaseProgram
    {
        protected static string GetApplicationInsightsInstrumentationKey(HostBuilderContext context)
        {
            string instrKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
            if (string.IsNullOrWhiteSpace(instrKey))
            {
                instrKey = context.Configuration["APPINSIGHTS_CONNECTIONSTRING"];
            }
            if (string.IsNullOrWhiteSpace(instrKey))
            {
                instrKey = context.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
            }
            if (string.IsNullOrWhiteSpace(instrKey))
            {
                instrKey = "3ab60130-3b80-4865-9939-d05e99ba9798";
            }
            return instrKey;
        }
    }
}
