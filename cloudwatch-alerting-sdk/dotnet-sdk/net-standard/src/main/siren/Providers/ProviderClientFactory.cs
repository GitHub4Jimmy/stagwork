using StagwellTech.SirenSDK.Configs;
using StagwellTech.SirenSDK.Exceptions;
using StagwellTech.SirenSDK.Providers.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;

namespace StagwellTech.SirenSDK.Providers
{
    internal static class ProviderClientFactory
    {

        public static BaseProviderClient CreateClient (SirenConfig config)
        {

            if (IsConfigTypeApplicationInsights(config)) return new ApplicationInsightsClient(config);

            throw new ConfigInvalidException("Provider is not specified or is missing critical values.");
        }

        private static bool IsConfigTypeApplicationInsights(SirenConfig config)
        {
            if (config.applicationInsights != null)
                if (String.IsNullOrWhiteSpace(config.applicationInsights.instrumentationKey) == false)
                    return true;

            return false;
        }
    }
}
