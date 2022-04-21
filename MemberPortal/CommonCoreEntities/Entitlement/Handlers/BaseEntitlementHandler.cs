using Microsoft.Extensions.DependencyInjection;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using System.Collections.Generic;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers
{

    public abstract class BaseEntitlementHandler : IEntitlementHandler
    {
        public static Dictionary<string, IEntitlementHandler> Handlers = new Dictionary<string, IEntitlementHandler>();
        protected BaseEntitlementHandler(string domain)
        {
            //Handlers.Add(domain, this);
        }
        public static IEntitlementHandler GetHandler(IServiceScope scope, string domain)
        {
            return GetHandler(scope.ServiceProvider, domain);
        }
        public static IEntitlementHandler GetHandler(System.IServiceProvider ServiceProvider, string domain)
        {
            if (domain == EntitlementTypes.EligibilityTypes.DOMAIN)
            {
                return ServiceProvider.GetService<EligibilityEntitlementHandler>();
            }
            if (domain == EntitlementTypes.CommunicationTypes.DOMAIN)
            {
                return ServiceProvider.GetService<CommunicationEntitlementHandler>();
            }
            if (domain == EntitlementTypes.DocumentTypes.DOMAIN)
            {
                return ServiceProvider.GetService<DocumentEntitlementHandler>();
            }
            return null;
        }
        public abstract IList<EntitlementPermission> handleRequest(EntitlementRequest eRequest, IList<string> entitlements);
    }
}
