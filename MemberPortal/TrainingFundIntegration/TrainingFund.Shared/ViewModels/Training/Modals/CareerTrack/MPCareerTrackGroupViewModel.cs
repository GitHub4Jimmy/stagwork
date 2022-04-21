using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.CareerTrack
{
    public class MPCareerTrackGroupViewModel : ITranslatable
    {
        [Translatable]
        public string GroupName { get; set; }
        [Translatable]
        public string CompletePhrase { get; set; }
        public bool isComplete { get; set; }
        public int CompleteCount { get; set; }
        [Translatable]
        public List<MPCareerTrackCourseViewModel> Courses { get; set; }
    }
}