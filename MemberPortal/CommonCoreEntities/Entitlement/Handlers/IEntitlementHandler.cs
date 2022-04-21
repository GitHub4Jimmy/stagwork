using StagwellTech.SEIU.CommonEntities.Entitlement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers
{
    public interface IEntitlementHandler
    {
        IList<EntitlementPermission> handleRequest(EntitlementRequest eRequest, IList<string> entitlements);
    }
}
