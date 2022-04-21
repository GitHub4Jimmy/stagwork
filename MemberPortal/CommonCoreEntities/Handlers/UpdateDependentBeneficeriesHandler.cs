using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.SEIU.CommonEntities.DataModels.Enums;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Dependent;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Handlers
{
    public class UpdateDependentBeneficeriesHandler : IRequestHandler<MPDependentPerson, MPDependentPerson>
    {
        readonly SeiuContext Context;

        public UpdateDependentBeneficeriesHandler(SeiuContext context)
        {
            Context = context;
        }

        public MPDependentPerson Handle(MPDependentPerson request)
        {
            throw new NotImplementedException();
        }

        public MPDependentPerson Handle()
        {
            throw new NotImplementedException();
        }

        public async Task<MPDependentPerson> HandleAsync(MPDependentPerson request)
        {
            var record = Context.MPDependentPersons.FirstOrDefault(x => x.PersonId == request.PersonId);

            if (record == null) return null;

            if (record.ToHide == true) return null;

            var entry = Context.Entry(record);

            var entity = entry.Entity;
            entity.HealthIsEligible = request.HealthIsEligible;
            entity.LegalIsEligible = request.LegalIsEligible;
            entity.LifeBeneficiary = request.LifeBeneficiary;
            entity.LifeIsEligible = request.LifeIsEligible;
            entity.PensionBeneficiary = request.PensionBeneficiary;

            await Context.SaveChangesAsync();
            return entry.Entity;
        }

        public Task<MPDependentPerson> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
