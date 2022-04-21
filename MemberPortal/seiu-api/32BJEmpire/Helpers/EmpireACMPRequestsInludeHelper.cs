using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.DataModels.Empire;

namespace _32BJEmpire.Helpers
{
    public static class EmpireACMPRequestsInludeHelper
    {
        public static IIncludableQueryable<EmpireACMPRequest, List<ProviderList>> IncludeAllEmpireRequestRelations(DbSet<EmpireACMPRequest> EmpireACMPRequests)
        {
            return EmpireACMPRequests
                .Include(x => x.authorizationDetails)
                .ThenInclude(a => a.fundActionNeeded)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.placeOfService)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.diagnosisCodeList)
                .ThenInclude(d => d.diagnosisCode)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.member)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.serviceList)
                .ThenInclude(s => s.procedureCode)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.serviceList)
                .ThenInclude(s => s.placeOfServiceCode)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.serviceList)
                .ThenInclude(s => s.serviceDecision)
                .ThenInclude(s => s.decisionCode)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.serviceList)
                .ThenInclude(s => s.serviceDecision)
                .ThenInclude(s => s.decisionReasonCode)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.serviceList)
                .ThenInclude(s => s.additionalProperties)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.serviceList)
                .ThenInclude(s => s.servicingProvider)
                .ThenInclude(s => s.additionalProperties)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.lengthOfStayList)
                .ThenInclude(a => a.levelOfcare)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.lengthOfStayList)
                .ThenInclude(a => a.losDecision)
                .ThenInclude(a => a.decisionCode)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.lengthOfStayList)
                .ThenInclude(a => a.losDecision)
                .ThenInclude(a => a.decisionReasonCode)
                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.lengthOfStayList)
                .ThenInclude(a => a.losDecision)
                .ThenInclude(a => a.levelOfcare)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.lengthOfStayList)
                .ThenInclude(a => a.additionalProperties)

                .Include(a => a.authorizationDetails)
                .ThenInclude(a => a.providerList);
        }
    }
}
