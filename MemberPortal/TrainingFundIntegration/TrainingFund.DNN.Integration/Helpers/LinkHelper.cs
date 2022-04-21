using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Helpers
{
    public class LinkHelper
    {
        private static Dictionary<string, Type> _providers;

        private static Dictionary<string, Type> Providers
        {
            get
            {
                if (_providers == null)
                {
                    _providers = GetAllEntities();
                }

                return _providers;
            }
        }

        private static string _DefaultProvider = "PlainLinkProvider";

        public static string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "", string defaultText = null)
        {
            if (link == null)
            {
                var txt = defaultText ?? "[Empty Link]";
                return $"<a href=\"#\">{txt}</a>";
            }
                
            ILinkProvider provider = GetProvider(link);

            return provider.GetLink(link, cssClass, textAppend);
        }

        public static LinkAttributesViewModel GetAttributes(MPGenericLinkButtonViewModel link)
        {
            ILinkProvider provider = GetProvider(link);

            return provider.GetLinkAttributes(link);
        }

        public static string GetUrl(MPGenericLinkButtonViewModel link)
        {
            ILinkProvider provider = GetProvider(link);

            return provider.GetUrl(link);
        }

        public static string GetLinkWithJumpUrl(MPGenericLinkButtonViewModel link)
        {
            var settings = DummyContentSettingsHelper.GetSettings();
            if (link.DatabaseName == LinkIdentifiers.MODAL_ENROLLED)
            {
                var url = LinkHelper.GetDnnUrl(KeyIdentifiers.PAGE_TRANSCIPTS_KEY);
                var href = !String.IsNullOrEmpty(url) ? url : "#";
                var id = link.DatabaseIds[KeyIdentifiers.MODAL_ENROLLED_KEY];
                return $"{href}?open-enrolled-popup={id}";
            }

            if (link.DatabaseName == LinkIdentifiers.MODAL_CAREER_TRACK)
            {
                var url = LinkHelper.GetDnnUrl(KeyIdentifiers.PAGE_CAREER_TRACK_KEY);
                var href = !String.IsNullOrEmpty(url) ? url : "#";
                var id = link.DatabaseIds[KeyIdentifiers.MODAL_CAREER_TRACK_KEY];
                return $"{href}?open-career-track-popup={id}";
            }

            if (link.DatabaseName == LinkIdentifiers.MODAL_COURSE)
            {
                var url = LinkHelper.GetDnnUrl(KeyIdentifiers.PAGE_FIND_TRAINING_COURSE_KEY);
                var href = !String.IsNullOrEmpty(url) ? url : "#";
                var id = link.DatabaseIds[KeyIdentifiers.MODAL_COURSE_KEY];
                return $"{href}?open-course-popup={id}";
            }

            return GetUrl(link);
        }

        private static ILinkProvider GetProvider(MPGenericLinkButtonViewModel link)
        {
            Type type = (link?.DatabaseName != null && Providers.ContainsKey(link.DatabaseName)) ? Providers[link.DatabaseName] : Providers[_DefaultProvider];

            ILinkProvider provider = (ILinkProvider)Activator.CreateInstance(type);

            return provider;
        }

        private static Dictionary<string, Type> GetAllEntities()
        {
            var type = typeof(ILinkProvider);

            var types = Assembly.GetAssembly(type).GetTypes()
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .ToDictionary(t => t.Name, t => t);

            return types;

        }

        public static string GetDnnUrl(string pageName)
        {
            int portalId = DotNetNuke.Common.Globals.GetHostPortalSettings().PortalId;
            int? tabId = GetTabId(pageName, portalId);

            if (tabId != null)
            {
                return DotNetNuke.Common.Globals.NavigateURL((int)tabId);
            }

            return String.Empty;
        }

        public static int? GetTabId(string tabName, int portalId)
        {
            TabController tc = new TabController();

            if (tc.GetTabByName(tabName, portalId) is TabInfo tabInfo)
            {
                var lc = new LocaleController();
                var locale = lc.GetCurrentLocale(portalId);

                if (tc.GetTabByCulture(tabInfo.TabID, portalId, locale) is TabInfo cultureTabInfo)
                {
                    return cultureTabInfo.TabID;
                }
            }   

            return null;
        }
    }
}
