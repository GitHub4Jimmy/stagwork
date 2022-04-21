using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionViewModel : ITranslatable
    {
        public int SessionId { get; set; }
        [Translatable]
        public List<MPSessionPropertyViewModel> Properties { get; set; }
        [Translatable]
        public List<FilterBoxViewModel> FilterProperties { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Actions { get; set; }
    }
}