using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using TrainingFund.DNN.Integration.Domain;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Interfaces;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class DirectionsLinkProvider : BaseProvider, ILinkProvider
    {
        private const string OriginPlaceHolder = "{origin}";
        private const string DestinationPlaceHolder = "{destination}";

        public string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "")
        {
            return $"<a {GetHref(link)} {GetClassAttribute(cssClass)} {GetTargetAttribute(link.isNewWindow)} {GetEnabledAttribute(link.isEnabled)}>{link.Text}{textAppend}</a>";
        }

        public LinkAttributesViewModel GetLinkAttributes(MPGenericLinkButtonViewModel link)
        {
            return new LinkAttributesViewModel()
            {
                Link = GetHref(link),
                Target = GetTargetAttribute(link.isNewWindow),
                Enabled = GetEnabledAttribute(link.isEnabled),
                Url = GetUrl(link)
            };
        }

        public string GetUrl(MPGenericLinkButtonViewModel link)
        {
            var baseUrls = Utilities.GetTraingFundEndpoints();

            if (baseUrls.ContainsKey(KeyIdentifiers.URLS_LOCATION_DIRECTIONS_KEY))
            {
                var destination = link.DatabaseIds.ContainsKey(KeyIdentifiers.LOCATIONS_DIRECTION_KEY)
                    ? link.DatabaseIds[KeyIdentifiers.LOCATIONS_DIRECTION_KEY]
                    : String.Empty;

                var origin = link.DatabaseIds.ContainsKey(KeyIdentifiers.LOCATIONS_DIRECTION_HOME_KEY)
                    ? link.DatabaseIds[KeyIdentifiers.LOCATIONS_DIRECTION_HOME_KEY]
                    : String.Empty;

                return baseUrls[KeyIdentifiers.URLS_LOCATION_DIRECTIONS_KEY]
                    .Replace(OriginPlaceHolder, origin)
                    .Replace(DestinationPlaceHolder, destination);
            }

            return "#";
        }

        private string GetHref(MPGenericLinkButtonViewModel link)
        {
            var href = GetUrl(link);

            return $"href=\"{href}\"";
        }
    }
}
