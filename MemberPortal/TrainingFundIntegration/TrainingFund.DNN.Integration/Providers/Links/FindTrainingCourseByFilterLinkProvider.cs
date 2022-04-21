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
    public class FindTrainingCourseByFilterLinkProvider : BaseProvider, ILinkProvider
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
               Target = GetTargetAttribute(link.isNewWindow),
               Enabled = GetEnabledAttribute(link.isEnabled),
               Url = GetUrl(link)
           };
        }

        public string GetUrl(MPGenericLinkButtonViewModel link)
        {
            var dnnLink = LinkHelper.GetDnnUrl(KeyIdentifiers.PAGE_FIND_TRAINING_COURSE_KEY);

            if (!String.IsNullOrEmpty(dnnLink)
                && link.DatabaseIds.ContainsKey(KeyIdentifiers.COURSE_FILTER_KEY))
            {
                var queryParameter =
                    $"{TrainingFundSearchHelper.FILTER_PRE_APPEND}{link.DatabaseIds[KeyIdentifiers.COURSE_FILTER_KEY]}";

                int idCount = 1;
                var value = String.Empty;

                while (link.DatabaseIds.ContainsKey($"{KeyIdentifiers.COURSE_SEARCH_KEY}{idCount}"))
                {
                    value += $"{link.DatabaseIds[KeyIdentifiers.COURSE_SEARCH_KEY + idCount]}|";
                    idCount++;
                }

                value = value.Trim('|');

                return $"{dnnLink}?{queryParameter}={value}";
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
