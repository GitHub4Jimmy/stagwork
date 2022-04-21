using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.API.DependentAPI;
using StagwellTech.SEIU.CommonCoreEntities.Handlers;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.ServiceBusRPC.Entities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UnitTests.DependentsTests
{
    public class AddDependentEnrollmentFormHandler : IRequestHandler<PortalEnrollmentForm, PortalEnrollmentForm>
    {
        private readonly IDependentService Service;
        private readonly ILogger<DependentBusController> Logger;

        public AddDependentEnrollmentFormHandler(IDependentService service, ILogger<DependentBusController> logger)
        {
            Service = service;
            Logger = logger;
        }

        public PortalEnrollmentForm Handle(PortalEnrollmentForm request)
        {
            throw new NotImplementedException();
        }

        public PortalEnrollmentForm Handle()
        {
            throw new NotImplementedException();
        }

        public async Task<PortalEnrollmentForm> HandleAsync(PortalEnrollmentForm request)
        {
            try
            {
                var result = await Service.AddEnrollmentForm(request);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                Logger.LogError(e.Message + " " + e.StackTrace);
            }
            return null;
        }

        public Task<PortalEnrollmentForm> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}