using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Course
{
    public class MPCoursePrereqViewModel : ITranslatable
    {
        public int CourseId { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel CourseName { get; set; }
        public bool isComplete { get; set; }
        public bool isPending { get; set; }
        [Translatable]
        public string Message { get; set; }
        public int Group { get; set; }
    }
}