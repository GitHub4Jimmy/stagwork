using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class PlainLinkProvider : BaseProvider, ILinkProvider
    {
        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass, link)} {GetTargetAttribute(link.isNewWindow)}  {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
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
            var url = link.Link;

            if (link.isEnabled.HasValue
                && link.isEnabled.Value == false)
            {
                url = "#";
            }

            return url;
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            var url = GetUrl(link);

            return $"href=\"{url}\"";
        }
    }
}
