using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;
using TrainingFund.Shared.ViewModels.Training.Modals.Session;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Course
{
    public class MPCourseDetailsAndSessionsViewModel : ITranslatable
    {
        [Translatable]
        public MPCourseDetailsViewModel CourseDetails { get; set; }
        [Translatable]
        public MPSessionsBoxViewModel Sessions { get; set; }
    }
}