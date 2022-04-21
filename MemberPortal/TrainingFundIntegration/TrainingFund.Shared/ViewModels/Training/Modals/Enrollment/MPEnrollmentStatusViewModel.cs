using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Enrollment
{
    public class MPEnrollmentStatusViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        [Translatable]
        public string StatusHeader { get; set; }
        [Translatable]
        public string Status { get; set; }
        public bool isStatusCheckMark { get; set; }
    }
}