using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.CareerTrack
{
    public class MPCareerTrackCourseViewModel : ITranslatable
    {
        public int CourseId { get; set; }
        public int EnrollmentId { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel CourseName { get; set; }
        [Translatable]
        public string SubName { get; set; }
        public bool isComplete { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Actions { get; set; }
    }
}