using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.HomePage
{
    public class MPTrainingHomeBoxItemViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        public bool isFlair { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Links { get; set; }
    }
}
