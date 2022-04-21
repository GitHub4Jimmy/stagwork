using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using StagwellTech.SEIU.CommonDNNEntities.ViewModels;
using StagwellTech.SEIU.CommonEntities.DataModels.ReadOnly.Event;
using StagwellTech.SEIU.CommonEntities.Utils;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using CommonUtilities = StagwellTech.SEIU.CommonEntities.Helpers.Utilities;
using ReadOnlyEvent = StagwellTech.SEIU.CommonEntities.ReadOnly.Event.Event;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public static class Utilities
    {
        public static int GetTabId(int PortalId, string tabname)
        {
            DotNetNuke.Entities.Tabs.TabController tc = new DotNetNuke.Entities.Tabs.TabController();

            return tc.GetTabByName(tabname, PortalId).TabID;

        }
        public static Dictionary<string, string> GetWebAPIUrls()
        {
            return PortalUrls.GetWebAPIUrls();
        }

        public static string GetDocumentsAPIEndpoint()
        {
            var apis = GetWebAPIUrls();
            string res;
            apis.TryGetValue("SEIU_API_documentsDomain", out res);
            return res;
        }

        public static string GetToken(HttpRequest request)
        {
            return request.Cookies.Get(".DOTNETNUKE").Value;
        }

        public static Dictionary<string, string> GetTraingFundEndpoints()
        {
            return PortalUrls.GetTraingFundEndpoints();
        }

        public static IDictionary<int, string> GetAllEnumerations<TEnum>() where TEnum : struct
        {
            var enumerationType = typeof(TEnum);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumeration type is expected.");

            var dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(enumerationType))
            {
                var name = Enum.GetName(enumerationType, value);
                dictionary.Add(value, name);
            }

            return dictionary;
        }

        public static string AddQueryParam(HttpRequest request, string name, string value, bool replaceOld = true)
        {
            var uriBuilder = new UriBuilder(request.Url.AbsoluteUri);
            var paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (replaceOld)
            {
                paramValues.Remove(name);
            }
            paramValues.Add(name, value);
            uriBuilder.Query = paramValues.ToString();

            return uriBuilder.Uri.AbsoluteUri;
        }

        public static string GetDisplayAddress(string country, string state, string county, string city, string postalCode, params string[] addressLines)
        {
            return CommonUtilities.GetDisplayAddress(country, state, county, city, postalCode, addressLines);
            //StringBuilder sb = new StringBuilder();
            //addressLines = addressLines.Where(al => !string.IsNullOrWhiteSpace(al)).ToArray();
            //var line1 = string.Join(", ", addressLines);
            //sb.Append(line1);
            //sb.Append("\n");
            //sb.Append(city);
            //sb.Append(county);
            //sb.Append(", ");
            //var line3 = string.Join(" ", state, postalCode);
            //sb.Append(line3);
            //if (!string.IsNullOrEmpty(country))
            //{
            //    sb.Append(", ");
            //    sb.Append(country);
            //}

            //return sb.ToString();
        }
        public static EventWrap WrapEvent(ReadOnlyEvent data)
        {
            return CommonUtilities.WrapEvent(data);
        }

        public static GeoCoordinate GetGeoCoordinate(decimal? latitude, decimal? longitude)
        {
            if (latitude.HasValue && longitude.HasValue)
            {
                double lat = (double)latitude.GetValueOrDefault();
                double lon = (double)longitude.GetValueOrDefault();

                return new GeoCoordinate(lat, lon);
            }
            return null;
        }

        /// <summary>
        /// Calculates the age in years of the current System.DateTime object on a later date.
        /// </summary>
        /// <param name="birthDate">The date of birth</param>
        /// <param name="laterDate">The date on which to calculate the age.</param>
        /// <returns>Age in years on a later day. 0 is returned as minimum.</returns>
        public static int GetCurrentAge(DateTime birthDate, DateTime laterDate)
        {
            int age;
            age = laterDate.Year - birthDate.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(laterDate.Date < birthDate.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }

            return age;
        }

        public static bool IsMatchingListElements<T>(List<T> firstList, List<T> secondList)
        {
            return firstList.Any(x => secondList.Any(y => y.Equals(x)));
        }

        public static void HideWrapperForEmptyField(RepeaterItemEventArgs e, string fieldName, string fieldValue)
        {
            if (!fieldValue.HasValue())
            {
                var panel = e.Item.FindControl($"pnl{fieldName}") as Panel;
                if (panel != null)
                {
                    panel.Visible = false;
                }
            }
        }

        public static void HideWrapperForEmptyFieldAll<T>(RepeaterItemEventArgs e, T item)
        {
            var props = item.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(string))
                {
                    HideWrapperForEmptyField(e, prop.Name, (string)prop.GetValue(item));
                }
            }
        }

        public static List<TabInfo> GetTabsByRolesAndTabMode(string[] roles, string TabMode, PortalSettings ps, int portalId, UserInfo userInfo)
        {
            int ParentIndex = 0;
            if (TabMode.Equals("Child"))
            {
                ParentIndex = ps.ActiveTab.Level;
            }
            else if (TabMode.Equals("GrandParent"))
            {
                ParentIndex = ps.ActiveTab.Level - 2;
            }
            else
            {
                TabMode = "SameLevel";
                ParentIndex = ps.ActiveTab.Level - 1;
            }

            if(ParentIndex < 0) ParentIndex = 0;

            var parent = ((TabInfo)ps.ActiveTab.BreadCrumbs[ParentIndex]);
            bool hasChildren = parent.HasChildren;

            var tabs = TabController.GetTabsByParent(parent.TabID, portalId).FindAll(
                 delegate (TabInfo tab)
                 {
                     if (!tab.IsDeleted)
                     {
                         if (userInfo.IsSuperUser)
                         {
                             return true;
                         }
                         else if (tab.IsVisible == false)
                         {
                             return false;
                         }
                         else
                         {
                             var permissions = tab.TabPermissions;

                             var rolePerms = permissions.Where(p => roles.Contains(p.RoleName)).ToList();
                             bool hasRestrictedAccess = rolePerms.Where(p => p.PermissionKey == "VIEW" && p.AllowAccess == false).Any();
                             bool hasAllowedAccess = rolePerms.Where(p => p.PermissionKey == "VIEW" && p.AllowAccess).Any();


                             return !hasRestrictedAccess && hasAllowedAccess;
                         }
                     }
                     return false;
                 }
            );
            return tabs;
        }

        public static List<TabBarViewModel> GetTabBarViewModels(List<TabInfo> tabs, int portalId, int tabId)
        {
            var res = new List<TabBarViewModel>();
            foreach (var t in tabs)
            {
                //Determine if this tab is active
                bool isActive;
                if (t.TabID == tabId)
                {
                    isActive = true;
                }
                else
                {
                    var children = TabController.GetTabsByParent(t.TabID, portalId);
                    isActive = children.Any(c => c.TabID == tabId);
                }

                //Build TabBarViewModel from this tab data
                var viewTab = new TabBarViewModel
                {
                    FullUrl = t.FullUrl,
                    Title = !String.IsNullOrEmpty(t.Title) ? t.Title : t.TabName,
                    IsActiveClass = isActive ? "active" : "",
                    IsActiveAttribute = isActive ? "selected" : ""
                };
                res.Add(viewTab);
            }
            return res;
        }


    }
}
