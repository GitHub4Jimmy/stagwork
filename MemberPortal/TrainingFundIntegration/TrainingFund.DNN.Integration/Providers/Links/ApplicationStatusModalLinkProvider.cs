using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using StagwellTech.SEIU.CommonEntities.BarCodeGenerator;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class ApplicationStatusModalLinkProvider : BaseProvider, ILinkProvider
    {
        //private readonly string UrlKey = "EnrollmentDetails";

        public static string ATTRIBUTE_KEY = "data-application-status-id";

        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass)} {GetTargetAttribute(link.isNewWindow)} {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
        }

        public LinkAttributesViewModel GetLinkAttributes(MPGenericLinkButtonViewModel link)
        {
            Dictionary<string, string> dataAttributes = new Dictionary<string, string>();

            dataAttributes.Add(ATTRIBUTE_KEY, link.DatabaseIds[KeyIdentifiers.APPLICATION_ID_KEY]);

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
            return (link.DatabaseIds.ContainsKey(KeyIdentifiers.APPLICATION_ID_KEY))
                ? $"href =\"#\" {ATTRIBUTE_KEY}=\"{link.DatabaseIds[KeyIdentifiers.APPLICATION_ID_KEY]}\""
                : "href=\"#\"";
        }

        public string GetUrl(MPGenericLinkButtonViewModel link)
        {
            var baseUrls = Utilities.GetTraingFundEndpoints();

            if (link.DatabaseIds.ContainsKey(KeyIdentifiers.APPLICATION_ID_KEY)
                && baseUrls.ContainsKey(KeyIdentifiers.URLS_SCHOLARSHIP_APPLICATION_STATUS_KEY))
            {
                var baseUrl = baseUrls[KeyIdentifiers.URLS_SCHOLARSHIP_APPLICATION_STATUS_KEY];
                return $"{baseUrl}{link.DatabaseIds[KeyIdentifiers.APPLICATION_ID_KEY]}";
            }

            return "#";
        }
    }
}
