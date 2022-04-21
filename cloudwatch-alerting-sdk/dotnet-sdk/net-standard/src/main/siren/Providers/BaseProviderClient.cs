using StagwellTech.SirenSDK.Configs;
using StagwellTech.SirenSDK.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StagwellTech.SirenSDK.Providers
{
    public abstract class BaseProviderClient
    {
        protected BaseProviderClient(SirenConfig config) { }

        public abstract void Log(string message);
        public abstract string GetLogs(ProviderClientFilter filter);
        public abstract void LogMetric(Metric metric);

    }

}
