using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.SEIU.CommonEntities.DataModels.Enums;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Handlers
{
    public class UpdateCrmFormHandler : IRequestHandler<DependentEnrollmentCrmForm, DependentEnrollmentCrmForm>
    {
        readonly SeiuContext Context;

        public UpdateCrmFormHandler(SeiuContext context)
        {
            Context = context;
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
            var record = Context.DependentsEnrollmentCrmForms.FirstOrDefault(x => x.Id == request.Id);
            if (record != null)
            {
                var entry = Context.Entry(record);
                entry.CurrentValues.SetValues(request);
                await Context.SaveChangesAsync();
                return entry.Entity;
            }
            else
            {
                var entity = await Context.AddAsync(request);
                await Context.SaveChangesAsync();
                return entity.Entity;
            }
        }

        public Task<DependentEnrollmentCrmForm> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
