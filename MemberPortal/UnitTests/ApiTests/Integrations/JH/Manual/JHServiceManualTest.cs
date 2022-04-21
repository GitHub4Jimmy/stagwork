using ApiTests.Services.JH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonCoreEntities.Services;
using StagwellTech.SEIU.CommonEntities.Portal.JH;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests1.Builders;

namespace StagwellTech.SEIU.CommonCoreEntities.Services.UnitTests
{
    [TestClass()]
    public class JHServiceManualTest : BaseTest
    {
        [TestMethod()]
        public async Task RequestNewDataTest()
        {
            var testPersonId = 1;
            BuildContext();
            TestDataBuilder builder = new TestDataBuilder(this.Context);
            builder.BuildJHSummaries()
                .BuildMPSSN(testPersonId, "043643191")
                .Build();

            var queue = new Mock<IBackgroundTaskQueue>();
            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            JHAsyncService jHAsyncService = new JHAsyncService(this.Context, queue.Object, serviceScopeFactory.Object);
            var response = await jHAsyncService.RequestNewData(1);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count > 0);
            Assert.IsTrue(this.Context.JHParticipantPlanSummaries.Count() > 1);
        }
    }
}