using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class ViewCertificateLinkProvider : BaseProvider, ILinkProvider
    {
        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass)} {GetTargetAttribute(link.isNewWindow)} {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
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

            if (baseUrls.ContainsKey(KeyIdentifiers.URLS_COURSE_VIEW_CERTIFCATE_KEY))
            {
                var url =
                    $"{baseUrls[KeyIdentifiers.URLS_COURSE_VIEW_CERTIFCATE_KEY]}{link.DatabaseIds[KeyIdentifiers.DOWNLOAD_ID_KEY]}";

                return url;
            }

            return "#";
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            var url = GetUrl(link);
            return $"href=\"{url}\"";
        }

    }
}
