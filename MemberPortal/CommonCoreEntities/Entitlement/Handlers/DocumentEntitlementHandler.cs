using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonCoreEntities.Clients;
using StagwellTech.SEIU.CommonEntities.DependencyHelper;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using StagwellTech.SEIU.CommonEntities.User;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers
{
    // TODO: Base class with static registry (map string:Handler)
    [SingletonDependency]
    public class DocumentEntitlementHandler : BaseEntitlementHandler
    {
        public DocumentEntitlementHandler() : base(EntitlementTypes.DocumentTypes.DOMAIN)
        {
        }

        public override IList<EntitlementPermission> handleRequest(EntitlementRequest eRequest, IList<string> entitlements)
        {
            List<EntitlementPermission> results = new List<EntitlementPermission>();
            foreach (string entitlement in entitlements)
            {
                var parts = entitlement.Split(".");
                var domain = parts[0];
                var permission = parts[1];

                if (domain != EntitlementTypes.DocumentTypes.DOMAIN)
                {
                    throw new ArgumentException("DocumentEntitlementHandler only handles entitlements of DOCUMENT domain");
                }

                if (permission == EntitlementTypes.DocumentTypes.FORM)
                {
                    results.Add(new EntitlementPermission()
                    {
                        Entitlement = entitlement,
                        Permission = EntitlementPermission.PermissionType.View
                    });
                }
                else if (permission == EntitlementTypes.DocumentTypes.DOCUMENT)
                {
                    results.Add(new EntitlementPermission()
                    {
                        Entitlement = entitlement,
                        Permission = EntitlementPermission.PermissionType.View
                    });
                }
                else if (permission == EntitlementTypes.DocumentTypes.CONTRACT)
                {
                    results.Add(new EntitlementPermission()
                    {
                        Entitlement = entitlement,
                        Permission = EntitlementPermission.PermissionType.View
                    });
                }

                results.Add(new EntitlementPermission()
                {
                    Entitlement = entitlement,
                    Permission = EntitlementPermission.PermissionType.None
                });
            }
            return results;
        }

    }
}
