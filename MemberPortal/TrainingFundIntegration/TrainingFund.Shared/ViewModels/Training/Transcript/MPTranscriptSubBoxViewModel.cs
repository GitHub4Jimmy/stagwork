using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPTranscriptSubBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string Column1 { get; set; }
        [Translatable]
        public string Column2 { get; set; }
        [Translatable]
        public string Column3 { get; set; }
        [Translatable]
        public List<MPTranscriptGridItemViewModel> TranscriptGridItems { get; set; }
        public int DisplayCount { get; set; }
        public int TranscriptGridItemCount { get; set; }
        public bool isVisible { get; set; }
        public bool isVisibleFooterLink { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel FooterLink { get; set; }
    }
}