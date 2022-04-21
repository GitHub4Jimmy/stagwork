using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using StagwellTech.SirenSDK.Configs;
using StagwellTech.SirenSDK.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SirenSDK.Providers.ApplicationInsights
{
    public class ApplicationInsightsClient : BaseProviderClient
    {

        private string URL = "https://api.applicationinsights.io/v1/apps/{appId}/query?query={query}";
        private TelemetryClient Writer { get; }
        private HttpClient Reader { get; }

        internal ApplicationInsightsClient(SirenConfig config) : base(config)
        {
            this.Writer = CreateWriter(config);
            this.Reader = CreateReader(config);
            URL = URL.Replace("{appId}", config.applicationInsights.appId);
        }

        private HttpClient CreateReader(SirenConfig config)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", config.applicationInsights.apiKey);
            return client;
        }

        private static TelemetryClient CreateWriter(SirenConfig config)
        {
            var tc = TelemetryConfiguration.CreateDefault();
            tc.InstrumentationKey = config.applicationInsights.instrumentationKey;
            tc.DisableTelemetry = !config.enabled;
            tc.TelemetryChannel.DeveloperMode = config.developerMode;

            var telemetryClient = new TelemetryClient(tc);
            telemetryClient.InstrumentationKey = tc.InstrumentationKey;
            return telemetryClient;
        }

        public override void Log(string message)
        {
            var t = new TraceTelemetry(message, SeverityLevel.Information);
            Writer.TrackTrace(t);
        }

        public override string GetLogs(ProviderClientFilter filter)
        {
            return GetTelemetry(filter.query);
        }

        private string GetTelemetry(string query)
        {
            URL = URL.Replace("{query}", query);

            HttpResponseMessage response = Reader.GetAsync(URL).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else
                return response.ReasonPhrase;
        }

        public override void LogMetric(Models.Metric metric)
        {
            Writer.GetMetric(metric.name).TrackValue(metric.value);
        }

    }
}
