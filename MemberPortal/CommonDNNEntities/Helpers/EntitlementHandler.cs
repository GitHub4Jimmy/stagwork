using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using StagwellTech.SEIU.CommonDNNEntities.DataProviders;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using TrainingFund.Shared.ViewModels;
using StagwellTech.SEIU.CommonEntities.Utils;
using StagwellTech.SirenSDK.Models;
using static DotNetNuke.Services.Log.EventLog.EventLogController;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public class EntitlementHandler
    {
        public static EntitlementHandler Instance { get; } = new EntitlementHandler();

        public async Task HandleEntitlements(UserInfo user, PortalSettings portalSettings, SEIUDNNContext context)
        {
            var sw = Stopwatch.StartNew();
            Debug.WriteLine("HandleEntitlements started =============================================================================");

            var canViewDict = await BuildCanViewDict(user.UserID, context, user, portalSettings);
            Debug.WriteLine("BuildCanViewDict took ===========> " + sw.ElapsedMilliseconds);

            AssignRoles(user, portalSettings, canViewDict);

            sw.Stop();
            Debug.WriteLine($"HandleEntitlements completed in {sw.ElapsedMilliseconds.ToString("# ###")} ms =============================================================================");
            MetricsLogger.Log($"handle-entitlements-auth0", sw.ElapsedMilliseconds);
            SEIUDNNContext.AddEventLog(user.DisplayName, user.UserID, "Member Portal", portalSettings.PortalName, EventLogType.ADMIN_ALERT, "HandleEntitlements took => " + sw.ElapsedMilliseconds, portalSettings.PortalId);

        }

        private async Task<Dictionary<string, bool>> BuildCanViewDict(long userId, SEIUDNNContext context, UserInfo user, PortalSettings portalSettings)
        {
            var resTask = context.GetEntitlements();

            var viewTrainingFundTask = context.CanViewTrainingFund(user, portalSettings);
            bool canViewTrainingFund = false;
            bool canViewScholarshipApplication = false;

            var canView = new Dictionary<string, bool>();

            await Task.WhenAll(resTask, viewTrainingFundTask);

            var res = resTask.Result;
            var viewTrainingFund = viewTrainingFundTask.Result;


            if (viewTrainingFund != null)
            {
                canViewTrainingFund = viewTrainingFund.IsTrainingMenuVisible;
                canViewScholarshipApplication = viewTrainingFund.IsScholarshipMenuVisible;
            }

            foreach (var item in EntitlementTypes.DEFAULT_ELIG_LIST)
            {
                if (item == EntitlementTypes.EligibilityTypes.Training)
                {
                    if (canView.ContainsKey(item))
                    {
                        canView[item] = canViewTrainingFund;
                    }
                    else
                    {
                        canView.Add(item, canViewTrainingFund);
                    }

                    continue;
                }

                if (item == EntitlementTypes.EligibilityTypes.Scholarship)
                {
                    if (canView.ContainsKey(item))
                    {
                        canView[item] = canViewScholarshipApplication;
                    }
                    else
                    {
                        canView.Add(item, canViewScholarshipApplication);
                    }

                    continue;
                }

                //If does not exist in response - assume false
                EntitlementPermission resItem = null;
                if (res != null)
                {
                    resItem = res.Entitlements.Where(e => e.Entitlement == item).FirstOrDefault();
                }

                if (resItem == null)
                {
                    canView.Add(item, false);
                }
                else //If exists in response - apply logic to extract value
                {
                    bool value = resItem.Permission != EntitlementPermission.PermissionType.None;

                    //Override existing or add a new one
                    if (canView.ContainsKey(resItem.Entitlement))
                    {
                        //If true, then override any existing false value
                        if (value == true)
                        {
                            canView[resItem.Entitlement] = true;
                        }
                    }
                    else
                    {
                        canView.Add(resItem.Entitlement, value);
                    }
                }
            }
            return canView;
        }

        private static void AssignRoles(UserInfo user, PortalSettings portalSettings, Dictionary<string, bool> canView)
        {
            RoleController roleController = new RoleController();

            IList<RoleInfo> portalRoles = roleController.GetRoles(portalSettings.PortalId);
            IList<UserRoleInfo> userRoles = roleController.GetUserRoles(user, true);

            foreach (var item in canView)
            {
                var role = portalRoles.Where(pr => pr.RoleName == item.Key).FirstOrDefault();
                if (role != null)
                {
                    bool hasRole = userRoles.Where(ur => ur.RoleID == role.RoleID).FirstOrDefault() != null;

                    //If user does not have a role but is entitled
                    if (!hasRole && item.Value)
                    {
                        //Add user to role
                        RoleController.AddUserRole(user, role, portalSettings, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
                        Debug.WriteLine($"{user.Username} was added to role: {role.RoleName}");
                    }
                    if (hasRole && !item.Value)
                    {
                        RoleController.DeleteUserRole(user, role, portalSettings, false);
                        Debug.WriteLine($"{user.Username} was removed from role: {role.RoleName}");
                    }
                }
            }
        }

    }
}
