using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPHomeBoxItemViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        public bool isFlair { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Links { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel FooterLink { get; set; }
    }
}