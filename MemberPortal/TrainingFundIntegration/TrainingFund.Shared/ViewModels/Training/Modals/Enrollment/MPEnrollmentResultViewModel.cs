using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;
using TrainingFund.Shared.ViewModels.Training.Transcript;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Enrollment
{
    public class MPEnrollmentResultViewModel : ITranslatable
    {
        public bool isSuccess { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public MPEnrollmentResultCourseBoxViewModel CourseDetails { get; set; }
        [Translatable]
        public MPRecommendationBoxViewModel Recommendations { get; set; }
        [Translatable]
        public MPResourceBoxViewModel Resources { get; set; }
    }
}