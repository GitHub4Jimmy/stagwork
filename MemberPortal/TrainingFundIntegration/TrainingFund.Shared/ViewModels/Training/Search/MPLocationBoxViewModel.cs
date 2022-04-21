using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;
using TrainingFund.Shared.ViewModels.Training.Transcript;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPLocationBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }

        [Translatable]
        public string FilterHeader { get; set; }

        [Translatable]
        public FilterBoxViewModel FilterBox { get; set; }
        [Translatable]
        public List<MPLocationViewModel> Locations { get; set; }
        public bool isVisible { get; set; }
        public bool isVisibleFilter { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel FooterLink { get; set; }
    }
}