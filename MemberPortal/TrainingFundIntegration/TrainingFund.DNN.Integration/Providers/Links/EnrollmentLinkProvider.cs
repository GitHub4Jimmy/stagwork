using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.Common.Utilities;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class EnrollmentLinkProvider : BaseProvider, ILinkProvider
    {
        //private readonly string UrlKey = "Enrollment";

        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass)} {GetTargetAttribute(link.isNewWindow)} {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
        }

        public LinkAttributesViewModel GetLinkAttributes(MPGenericLinkButtonViewModel link)
        {
            Dictionary<string, string> dataAttributes = new Dictionary<string, string>();

            dataAttributes.Add("data-enroll-now", $"{link.DatabaseIds[KeyIdentifiers.MODAL_SESSION_KEY]};{link.DatabaseIds[KeyIdentifiers.MODAL_COURSE_KEY]}");

            return new LinkAttributesViewModel()
            {
                Link = GetHref(link),
                Target = GetTargetAttribute(link.isNewWindow),
                Enabled = GetEnabledAttribute(link.isEnabled),
                Url = GetUrl(link),
                DataAttributes = dataAttributes
            };
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            return (link.DatabaseIds.ContainsKey(KeyIdentifiers.MODAL_COURSE_KEY))
                ? $"href=\"#\" data-enroll-now=\"{link.DatabaseIds[KeyIdentifiers.MODAL_SESSION_KEY]};{link.DatabaseIds[KeyIdentifiers.MODAL_COURSE_KEY]}\""
                : "href=\"#\"";
        }

        public string GetUrl(MPGenericLinkButtonViewModel link)
        {
            //var settings = GlobalSettingsHelper.GetSettings();
            var baseUrls = Utilities.GetTraingFundEndpoints();

            if (link.DatabaseIds.ContainsKey(KeyIdentifiers.MODAL_COURSE_KEY)
                && link.DatabaseIds.ContainsKey(KeyIdentifiers.MODAL_SESSION_KEY)
                && baseUrls.ContainsKey(KeyIdentifiers.URLS_COURSE_ENROLL_KEY))
            {
                var url = baseUrls[KeyIdentifiers.URLS_COURSE_ENROLL_KEY];

                url = url
                    .Replace($"{{{KeyIdentifiers.MODAL_COURSE_KEY}}}",
                        link.DatabaseIds[KeyIdentifiers.MODAL_COURSE_KEY])
                    .Replace($"{{{KeyIdentifiers.MODAL_SESSION_KEY}}}",
                        link.DatabaseIds[KeyIdentifiers.MODAL_SESSION_KEY]);

                return url;
            }

            return "#";
        }
    }
}
