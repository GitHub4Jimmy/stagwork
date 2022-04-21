using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.SEIU.CommonEntities.DataModels.Enums;
using System;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Handlers
{
    public class SubmitFormHandler : IRequestHandler<DependentEnrollmentCrmForm, DependentEnrollmentCrmForm>
    {

        public SubmitFormHandler()
        {
        }

        public DependentEnrollmentCrmForm Handle(DependentEnrollmentCrmForm request)
        {
            throw new NotImplementedException();
        }

        public DependentEnrollmentCrmForm Handle()
        {
            throw new NotImplementedException();
        }

        public async Task<DependentEnrollmentCrmForm> HandleAsync(DependentEnrollmentCrmForm request)
        {
            await DependentEnrollmentCrmClient.Instance.CreateEnrollmentForm(request);
            return request;
        }

        public Task<DependentEnrollmentCrmForm> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
