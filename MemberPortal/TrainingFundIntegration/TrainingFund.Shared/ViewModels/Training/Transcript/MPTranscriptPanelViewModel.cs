using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPTranscriptPanelViewModel : ITranslatable
    {
        [Translatable]
        public List<MPTranscriptBoxViewModel> Items { get; set; }
        public bool isVisible { get; set; }
    }
}