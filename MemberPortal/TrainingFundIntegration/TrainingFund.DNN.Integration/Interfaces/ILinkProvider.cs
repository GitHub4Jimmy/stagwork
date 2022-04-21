using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.ComponentModel;
using TrainingFund.DNN.Integration.ViewModels;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Interfaces
{
    interface ILinkProvider
    {
        string GetLink(MPGenericLinkButtonViewModel link, string cssClass = null, string textAppend = "");

        LinkAttributesViewModel GetLinkAttributes(MPGenericLinkButtonViewModel link);

        string GetUrl(MPGenericLinkButtonViewModel link);

    }
}
