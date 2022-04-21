using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.HomePage
{
    public class MPTrainingHomeBoxViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        public bool isVisibleContent { get; set; }

        [Translatable]
        public string SubHeader { get; set; }

        [Translatable]
        public string Content { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel ContentLink { get; set; }
        [Translatable]
        public List<MPTrainingHomeBoxItemViewModel> Items { get; set; }
    }
}