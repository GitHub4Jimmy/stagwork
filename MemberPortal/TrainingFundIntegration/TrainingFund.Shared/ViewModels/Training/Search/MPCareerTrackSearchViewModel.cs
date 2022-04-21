using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPCareerTrackSearchViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string Content { get; set; }
        [Translatable]
        public string EnrolledHeader { get; set; }
        [Translatable]
        public string AvailableHeader { get; set; }
        [Translatable]
        public List<MPCourseViewModel> Enrolled { get; set; }
        [Translatable]
        public List<MPCourseViewModel> Available { get; set; }
        public bool isVisibleEnrolled { get; set; }
        public bool isVisibleAvailable { get; set; }
    }
}