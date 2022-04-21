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
    public class CareerTrackModalLinkProvider : BaseProvider, ILinkProvider
    {
        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass)} {GetTargetAttribute(link.isNewWindow)} {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
        }

        public LinkAttributesViewModel GetLinkAttributes(MPGenericLinkButtonViewModel link)
        {
            Dictionary<string, string> dataAttributes = new Dictionary<string, string>();

            dataAttributes.Add("data-career-track-id", link.DatabaseIds[KeyIdentifiers.MODAL_CAREER_TRACK_KEY]);

            return new LinkAttributesViewModel()
            {
                Link = GetHref(link),
                Target = GetTargetAttribute(link.isNewWindow),
                Enabled = GetEnabledAttribute(link.isEnabled),
                Url = GetUrl(link),
                DataAttributes = dataAttributes
            };
        }

        public string GetUrl(MPGenericLinkButtonViewModel link)
        {
            var baseUrls = Utilities.GetTraingFundEndpoints();
            
            if (link.DatabaseIds.ContainsKey(KeyIdentifiers.MODAL_CAREER_TRACK_KEY)
                && baseUrls.ContainsKey(KeyIdentifiers.URLS_CAREER_TRACK_DETAILS_KEY))
            {
                var baseUrl = baseUrls[KeyIdentifiers.URLS_CAREER_TRACK_DETAILS_KEY];
                return $"{baseUrl}{link.DatabaseIds[KeyIdentifiers.MODAL_CAREER_TRACK_KEY]}";
            }

            return "#";
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            return (link.DatabaseIds.ContainsKey(KeyIdentifiers.MODAL_CAREER_TRACK_KEY))
                ? $"href=\"#\" data-career-track-id=\"{link.DatabaseIds[KeyIdentifiers.MODAL_CAREER_TRACK_KEY]}\""
                : "href=\"#\"";
        }
    }
}
