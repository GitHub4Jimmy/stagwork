using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StagwellTech.SEIU.CommonCoreEntities.Clients;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.DependencyHelper;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using StagwellTech.SEIU.CommonEntities.User;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers
{
    [SingletonDependency]
    public class CommunicationEntitlementHandler : BaseEntitlementHandler
    {
        private readonly CommunicationSettingsClient _communicationSettingsClient;
        private SeiuContext seiuContext;
        public CommunicationEntitlementHandler(CommunicationSettingsClient communicationSettingsClient, SeiuContext seiuContext) : base(EntitlementTypes.CommunicationTypes.DOMAIN)
        {
            _communicationSettingsClient = communicationSettingsClient;
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
            var settings = await seiuContext.UserSettings.Where(item => item.DNNUserId == eRequest.UserId).FirstAsync();
            long personId;

            if(!long.TryParse(settings.PersonId, out personId))
            {
                return new List<EntitlementPermission>();
            }

            foreach (string entitlement in entitlements)
            {
                CommunicationSetting result = null;
                var parts = entitlement.Split(".");
                var domain = parts[0];
                var permission = parts[1];
                try
                {
                    CommunicationType communicationType;
                    if (Enum.TryParse(permission, out communicationType))
                    {
                        var setting = new CommunicationSetting()
                        {
                            PersonId = personId,
                            Type = communicationType
                        };
                        result = await _communicationSettingsClient.Check(setting);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (result != null)
                {
                    results.Add(new EntitlementPermission()
                    {
                        Entitlement = entitlement,
                        Permission = EntitlementPermission.PermissionType.Execute
                    });
                }
                else
                {
                    results.Add(new EntitlementPermission()
                    {
                        Entitlement = entitlement,
                        Permission = EntitlementPermission.PermissionType.None
                    });
                }
            }

            return results;
        }
    }
}
