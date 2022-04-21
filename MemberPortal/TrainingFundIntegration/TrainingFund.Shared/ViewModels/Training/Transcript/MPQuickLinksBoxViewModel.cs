using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPQuickLinksBoxViewModel : ITranslatable
    {
        public bool isVisible { get; set; }

        [Translatable]
        public string Header { get; set; }

        [Translatable]
        public List<MPGenericLinkButtonViewModel> Links { get; set; }
    }
}