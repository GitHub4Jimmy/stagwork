using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPTranscriptGridItemViewModel : ITranslatable
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int SessionId { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel Column1 { get; set; }
        [Translatable]
        public string Column1Subscript { get; set; }
        [Translatable]
        public string Column2 { get; set; }
        [Translatable]
        public string Column2Subscript { get; set; }
        [Translatable]
        public string Column3 { get; set; }
        [Translatable]
        public string Column3Subscript { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Links { get; set; }
        public bool isDefaultExpanded { get; set; }
    }
}