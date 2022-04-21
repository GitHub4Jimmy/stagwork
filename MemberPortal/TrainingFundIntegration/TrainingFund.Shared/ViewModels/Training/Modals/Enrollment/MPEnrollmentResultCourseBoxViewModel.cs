using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Enrollment
{
    public class MPEnrollmentResultCourseBoxViewModel : ITranslatable
    {
        [Translatable]
        public string CourseName { get; set; }
        [Translatable]
        public string SessionLocation { get; set; }
        public string SessionDatesAndTimes { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Actions { get; set; }
    }
}