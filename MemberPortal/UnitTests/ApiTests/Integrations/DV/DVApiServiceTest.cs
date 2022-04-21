using ApiTests.Services.JH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonCoreEntities.Services;
using StagwellTech.SEIU.CommonEntities.DavisVision;
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
    public class DVApiServiceTest : BaseTest
    {
        [TestMethod()]
        public void GetMemberEligibilityTest()
        {
            DVApiClient dvClient = DVApiClient.Instance;
            var response = dvClient.GetMemberEligibility("078964369", "", "", "");

            Assert.IsNotNull(response);
            // TODO: Assert
        }
    }
}