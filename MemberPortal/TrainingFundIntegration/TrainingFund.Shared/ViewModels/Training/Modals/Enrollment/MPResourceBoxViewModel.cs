using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Enrollment
{
    public class MPResourceBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Links { get; set; }
        public bool isVisible { get; set; }
    }
}