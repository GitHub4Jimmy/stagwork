using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionsBoxViewModel : ITranslatable
    {
        public int CourseId { get; set; }
        [Translatable]
        public string CourseName { get; set; }
        [Translatable]
        public string SubHeader { get; set; }

        public bool isVisibleFilters { get; set; }
        [Translatable]
        public string FiltersHeader { get; set; }
        [Translatable]
        public List<FilterBoxViewModel> FilterBoxes { get; set; }
        [Translatable]
        public string SessionsHeader { get; set; }
        [Translatable]
        public List<MPSessionViewModel> Sessions { get; set; }
    }
}