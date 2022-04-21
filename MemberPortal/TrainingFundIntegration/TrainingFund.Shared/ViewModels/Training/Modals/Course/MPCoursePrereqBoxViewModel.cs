using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Course
{
    public class MPCoursePrereqBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public List<MPCoursePrereqViewModel> Prereqs { get; set; }
        public int Group { get; set; }
    }
}