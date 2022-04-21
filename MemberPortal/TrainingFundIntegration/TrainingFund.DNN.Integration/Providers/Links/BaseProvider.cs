using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Providers.Links
{
    public class BaseProvider
    {
        protected string GetTargetAttribute(bool newWindow)
        {
            return (newWindow) ? "target=\"_blank\"" : "";
        }

        protected string GetEnabledAttribute(bool? isEnabled)
        {
            if (isEnabled.HasValue
                && isEnabled.Value == false)
            {
                return "disabled";
            }

            return String.Empty;
        }

        protected string GetClassAttribute(string cssClass, MPGenericLinkButtonViewModel link = null)
        {
            var disabled = link != null && link.isEnabled.HasValue && link.isEnabled.Value == false ? " disabled" : String.Empty;
            var simpleClass = !String.IsNullOrEmpty(cssClass) ? cssClass : String.Empty;

            var fullCssClass = $"{simpleClass}{disabled}".Trim();

            if (!String.IsNullOrEmpty(fullCssClass))
            {
                return $"class=\"{fullCssClass}\"";
            }

            return String.Empty;
        }
    }
}
