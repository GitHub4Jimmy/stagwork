using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StagwellTech.SEIU.API.MPEligibilityAPI;
using StagwellTech.SEIU.CommonCoreEntities.Clients;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.DependencyHelper;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using StagwellTech.SEIU.CommonEntities.User;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers
{
    // TODO: Base class with static registry (map string:Handler)
    [SingletonDependency]
    public class DocumentAccessEntitlementHandler : BaseEntitlementHandler
    {
        private SeiuDNNContext dnnContext;
        private SeiuContext seiuContext;
        public DocumentAccessEntitlementHandler(SeiuDNNContext dnnContext, SeiuContext seiuContext) : base(EntitlementTypes.DocumentAccessTypes.DOMAIN)
        {
            this.dnnContext = dnnContext;
            this.seiuContext = seiuContext;
        }

        public override IList<EntitlementPermission> handleRequest(EntitlementRequest eRequest, IList<string> entitlements)
        {
            var task = Task.Run(() => _handleRequest(eRequest, entitlements));
            task.Wait(); // Not sure if needed
            return task.Result;
        }

        public async Task<IList<EntitlementPermission>> _handleRequest(EntitlementRequest eRequest, IList<string> entitlements)
        {
            List<EntitlementPermission> results = new List<EntitlementPermission>();

            var isAdmin = await dnnContext.DNNUsers.AnyAsync(user => user.UserID == eRequest.UserId && user.IsSuperUser);
            var settings = await seiuContext.UserSettings.Where(item => item.DNNUserId == eRequest.UserId).FirstAsync();
            foreach (string entitlement in entitlements)
            {
                var parts = entitlement.Split(".");
                var domain = parts[0];
                var permission = parts[1];

                if (domain != EntitlementTypes.DocumentAccessTypes.DOMAIN)
                {
                    throw new ArgumentException("DocumentEntitlementHandler only handles entitlements of DOCUMENT domain");
                }

                if (entitlement == EntitlementTypes.DocumentAccessTypes.Download)
                {
                    //eRequest.ResourceIds

                    if(settings == null)
                    {
                        continue;
                    }

                    if(isAdmin)
                    {
                        results.Add(new EntitlementPermission()
                        {
                            Entitlement = entitlement,
                            Permission = EntitlementPermission.PermissionType.View
                        });
                        continue;
                    }

                    //var documents = await seiuContext.Documents.Where(doc => eRequest.ResourceIds.Contains(doc.Id)).ToListAsync();

                    var contractIds = await seiuContext.UnionContracts
                        .Where(c => c.PersonID == int.Parse(settings.PersonId) && c.ContractID != null)
                        .Select(c => (long)c.ContractID.GetValueOrDefault())
                        .ToListAsync();

                    var planCodes = await MPEligibilityService.GetPlanCodesQuery(seiuContext, int.Parse(settings.PersonId)).Distinct().ToListAsync();


                    var allowedDocumentIds = await (
                        from doc in seiuContext.Documents
                        where
                            eRequest.ResourceIds.Contains(doc.Id) &&
                            (
                                doc.PersonId == int.Parse(settings.PersonId) ||
                                (doc.PersonId == null && (doc.PlanCode == null || doc.PlanCode == "") && doc.ContractId == null) ||
                                (doc.PersonId == null && (planCodes.Contains(doc.PlanCode))) ||
                                (doc.PersonId == null && (contractIds.Contains((long)doc.ContractId)))
                            )
                        select doc.Id
                    ).ToListAsync();

                    var allowed = true;

                    if (allowedDocumentIds != null && allowedDocumentIds.Count > 0) {
                        eRequest.ResourceIds.ForEach(id =>
                        {

                            if (!allowedDocumentIds.Contains(id))
                            {
                                allowed = false;
                            }

                        });
                    } else
                    {
                        allowed = false;
                    }

                    if (allowed)
                    {
                        results.Add(new EntitlementPermission()
                        {
                            Entitlement = entitlement,
                            Permission = EntitlementPermission.PermissionType.View
                        });
                    }
                }
                else if (entitlement == EntitlementTypes.DocumentAccessTypes.Upload)
                {
                    // TODO: check if admin
                    if (isAdmin)
                    {
                        results.Add(new EntitlementPermission()
                        {
                            Entitlement = entitlement,
                            Permission = EntitlementPermission.PermissionType.Admin
                        });
                    }
                }
            }
            return results;
        }

    }
}
