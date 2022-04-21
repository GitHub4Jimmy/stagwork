using Microsoft.VisualStudio.TestTools.UnitTesting;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.GoogleServices.GooglePay;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Services.UnitTests
{
    [TestClass()]
    public class GooglePayServiceTest : BaseTest
    {
        [TestMethod()]
        public async Task GooglePassLinkTest()
        {
            GooglePayService service = new GooglePayService();
            var result = await service.GetGooglePassUrl("id", "name", "district");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("ey"));
        }

        [TestMethod()]
        public async Task GenerateGooglePayAccessToken()
        {
            GooglePayService service = new GooglePayService();
            var result = await service.GenerateGooglePayAccessToken();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith('e'));
        }
    }
}