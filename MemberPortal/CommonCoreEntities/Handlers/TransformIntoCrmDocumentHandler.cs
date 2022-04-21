using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.API.DependentAPI;
using StagwellTech.SEIU.API.UserAPI;
using StagwellTech.SEIU.CommonCoreEntities.Handlers;
using StagwellTech.SEIU.CommonCoreEntities.Services.Interfaces;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DependentAPI.Handlers
{ 
    public class TransformIntoCrmDocumentHandler : IRequestHandler<PortalEnrollmentFormDocument, DependentsEnrollmentCrmDocument>
    {
        public DependentsEnrollmentCrmDocument Handle(PortalEnrollmentFormDocument request)
        {
            try
            {
                return DependentsEnrollmentCrmDocument.FromPortalDocument(request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            return null;
        }

        public DependentsEnrollmentCrmDocument Handle()
        {
            throw new NotImplementedException();
        }

        public async Task<DependentsEnrollmentCrmDocument> HandleAsync(PortalEnrollmentFormDocument request)
        {
            throw new NotImplementedException();
        }

        public Task<DependentsEnrollmentCrmDocument> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
