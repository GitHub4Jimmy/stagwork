using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPTranscriptBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }

        [Translatable]
        public List<MPGenericLinkButtonViewModel> Links { get; set; }

        [Translatable]
        public List<MPTranscriptSubBoxViewModel> TranscriptSubBoxes { get; set; }

        public bool isVisible { get; set; }
    }
}