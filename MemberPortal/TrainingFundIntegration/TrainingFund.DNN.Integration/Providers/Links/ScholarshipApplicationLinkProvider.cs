using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class ScholarshipApplicationLinkProvider : BaseProvider, ILinkProvider
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
            var url = LinkHelper.GetDnnUrl(KeyIdentifiers.PAGE_SCHOLARSHIP_APPLICATION_KEY);

            if (!String.IsNullOrEmpty(url))
            {
                var href = (link.DatabaseIds.ContainsKey(KeyIdentifiers.APPLICATION_ID_KEY))
                    ? $"{url}?applicationId={link.DatabaseIds[KeyIdentifiers.APPLICATION_ID_KEY]}"
                    : url;

                return href;
            }

            return "#";
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            return $"href=\"{GetUrl(link)}\"";
        }
    }
}
