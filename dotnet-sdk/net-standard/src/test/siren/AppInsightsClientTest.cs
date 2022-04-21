using Newtonsoft.Json.Linq;
using StagwellTech.SirenSDK;
using StagwellTech.SirenSDK.Configs;
using StagwellTech.SirenSDK.Models;
using StagwellTech.SirenSDK.Providers;
using StagwellTech.SirenTests.Utils;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Stagwelltech.SirenTests
{
    public class AppInsightsClientTest
    {

        [Fact]
        public void CreateClient()
        {
            var s = Siren.GetInstance(TestUtils.DefaultConfigPath);
            Assert.True(s != null);
        }

        [Fact]
        public void CreateClientFromInMemoryConfig()
        {
            var config = new SirenConfig { applicationInsights = new SirenConfig.AzureApplicationInsights { instrumentationKey = "dummyValue" } };
            var s = Siren.GetInstance(config);
            Assert.True(s != null);
        }

        [Fact]
        public void GetTelemetry()
        {
            var s = Siren.GetInstance(TestUtils.DefaultConfigPath);

            var filter = new ProviderClientFilter { query = "traces | where timestamp >= ago(0h) | count" };
            var logs = s.Provider.GetLogs(filter);

            dynamic data = JObject.Parse(logs);
            string parsedCount = data.tables[0].rows[0][0];

            Assert.True(parsedCount == "0");
        }

        [Fact]
        public void SendTelemetry()
        {
            var s = Siren.GetInstance(TestUtils.DefaultConfigPath);
            s.Provider.Log("UNIT_TEST. SendTelemetry.");

            Assert.True(true);
        }

        [Fact]
        public async void Z_SendTelemetryAndVerifyIfWasReceived()
        {
            var s = Siren.GetInstance(TestUtils.DefaultConfigPath);

            var guid = Guid.NewGuid();
            s.Provider.Log($"UNIT_TEST. SendTelemetryAndVerifyIfWasReceived. Guid = {guid}");

            var query = $"traces | where message contains '{guid}' | count";
            var wasTelimetryFound = await TryGetTelemetryRecursively(s, query, 5 * 60, 10);
            Assert.True(wasTelimetryFound);
        }

        static async Task<bool> TryGetTelemetryRecursively(Siren s, string query, int timeoutSeconds = 60, int callIntervalSeconds = 10)
        {
            var stopTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);

            while (stopTime >= DateTime.UtcNow)
            {
                var res = TryGetTelemetry(s.Provider, query);
                if (res > 0) return true;
                await Task.Delay(callIntervalSeconds * 1000);
            }

            return false;
        }

        static int TryGetTelemetry(BaseProviderClient client, string query)
        {
            var filter = new ProviderClientFilter { query = query };

            var logs = client.GetLogs(filter);

            dynamic data = JObject.Parse(logs);
            string parsedCount = data.tables[0].rows[0][0];

            return int.Parse(parsedCount);
        }

        [Fact]
        public async void LogTestMetricFor10Minutes()
        {
            var s = Siren.GetInstance(TestUtils.DefaultConfigPath);

            var endTime = DateTime.Now + TimeSpan.FromMinutes(10);

            Random rnd = new Random();

            while (endTime > DateTime.Now)
            {
                var m = new Metric
                {
                    name = "TestMetric",
                    value = rnd.Next(20, 80)
                };

                var m2 = new Metric
                {
                    name = "TestMetric2",
                    value = rnd.Next(0, 50)
                };

                s.Provider.LogMetric(m);
                s.Provider.LogMetric(m2);

                await Task.Delay(1000);
            }

        }

    }
}
