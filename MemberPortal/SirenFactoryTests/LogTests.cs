using AutoMapper;
using DotNetNuke.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StagwellTech.SEIU.CommonEntities.Utils;
using StagwellTech.SirenSDK.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SirenFactoryTests
{
    [TestClass]
    public class LogTests
    {
        [TestMethod]
        public void TestIfLogged()
        {
            SirenFactory.SirenDNN.Provider.LogMetric(new Metric("test", 1));
            var logs = SirenFactory.SirenDNN.Provider.GetLogs(new ProviderClientFilter()
            {
                query = "customMetrics | top 100 by timestamp"
            });
            var a = JsonSerializer.Deserialize<Log>(logs);
            Assert.IsNotNull(a);
            Assert.IsNotNull(a.Tables);
            Assert.IsTrue(a.Tables.Length > 0);

            SirenFactory.SirenAPI.Provider.LogMetric(new Metric("test", 1));
            logs = SirenFactory.SirenAPI.Provider.GetLogs(new ProviderClientFilter()
            {
                query = "customMetrics | top 100 by timestamp"
            });
            var b = JsonSerializer.Deserialize<Log>(logs);
            Assert.IsNotNull(b);
            Assert.IsNotNull(b.Tables);
            Assert.IsTrue(b.Tables.Length > 0);
        }
    }

    public class Log
    {
        [JsonPropertyName("tables")]
        public Table[] Tables { get; set; }
    }


    public class Rootobject
    {
        public Table[] tables { get; set; }
    }

    public class Table
    {
        public string name { get; set; }
        public Column[] columns { get; set; }
        public object[][] rows { get; set; }
    }

    public class Column
    {
        public string name { get; set; }
        public string type { get; set; }
    }

}
