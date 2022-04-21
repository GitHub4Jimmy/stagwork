using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Course
{
    public class MPCourseDescriptionViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string Description { get; set; }
    }
}