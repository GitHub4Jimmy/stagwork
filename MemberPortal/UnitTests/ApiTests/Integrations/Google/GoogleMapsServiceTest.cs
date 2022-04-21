using ApiTests.Services.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StagwellTech.SEIU.CommonEntities.DTO.Address;
using StagwellTech.SEIU.CommonEntities.GoogleServices;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.GoogleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Services.UnitTests
{
    [TestClass()]
    public class GoogleMapsServiceTest : BaseTest
    {
        [TestMethod()]
        public async Task GetLocationTest()
        {
            var request = new AddressRequest
            {
                PostalCode = "02189",
                Address1 = "Rodūnios Kl.",
                Address2 = "10A",
                City = "Vilnius",
                County = "Vilniaus",
                Country = "Lithuania"
            };
            var response = await GoogleMaps.GetLocationAsync(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Longitude);
            Assert.IsNotNull(response.Latitude);
            Assert.AreEqual(25, Math.Truncate(response.Longitude ?? 0));
            Assert.AreEqual(54, Math.Truncate(response.Latitude ?? 0));
        }
    }
}