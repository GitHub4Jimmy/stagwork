using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using TrainingFund.DNN.Integration.Domain;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class LaunchDoceboCourseLinkProvider : BaseProvider, ILinkProvider
    {
        private const string DoceboLaunchCourseBaseUrl = "DOCEBO_LAUNCH_COURSE_BASE_URL";

        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass)} {GetTargetAttribute(link.isNewWindow)}  {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
        }

        public LinkAttributesViewModel GetLinkAttributes(MPGenericLinkButtonViewModel link)
        {
            return new LinkAttributesViewModel()
            {
                Link = GetHref(link),
                Enabled = GetEnabledAttribute(link.isEnabled),
                Target = GetTargetAttribute(link.isNewWindow),
                Url = GetUrl(link)
            };
        }

        public string GetUrl(MPGenericLinkButtonViewModel link)
        {
            var baseUrls = Utilities.GetTraingFundEndpoints();
            var launchDoceboCourseBaseUrl = Environment.GetEnvironmentVariable(DoceboLaunchCourseBaseUrl) ?? baseUrls["DoceboBaseUrl"];

            var redirectUrl = HttpUtility.UrlEncode($"{launchDoceboCourseBaseUrl}{link.Link}");

            return $"{baseUrls[KeyIdentifiers.URLS_DOCEBO_LAUNCH_COURSE_KEY]}{redirectUrl}";
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            return $"href=\"{GetUrl(link)}\"";
        }
    }
}
