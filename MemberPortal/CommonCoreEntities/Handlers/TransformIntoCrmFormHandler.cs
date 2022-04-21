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
    public class TransformIntoCrmFormHandler : IRequestHandler<PortalEnrollmentForm, DependentEnrollmentCrmForm>
    {
        private readonly IContactsService ContactsService;

        public TransformIntoCrmFormHandler (IContactsService contactsService)
        {
            ContactsService = contactsService;
        }

        public DependentEnrollmentCrmForm Handle(PortalEnrollmentForm request)
        {
            throw new NotImplementedException();
        }

        public DependentEnrollmentCrmForm Handle()
        {
            throw new NotImplementedException();
        }

        public async Task<DependentEnrollmentCrmForm> HandleAsync(PortalEnrollmentForm request)
        {
            try
            {
                var parentContact = await ContactsService.GetContactByPersonId(request.PersonId.ToString());
                var contact = await ContactsService.GetContactByPersonId(request.DependantPersonId.ToString());
                return DependentEnrollmentCrmForm.FromPortalForm(request, contact, parentContact);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            return null;
        }

        public Task<DependentEnrollmentCrmForm> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
