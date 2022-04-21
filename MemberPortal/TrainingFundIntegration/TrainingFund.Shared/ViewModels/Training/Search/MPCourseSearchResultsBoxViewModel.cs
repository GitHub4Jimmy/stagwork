using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPCourseSearchResultsBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public List<MPCourseSearchResultViewModel> Items { get; set; }
        public int DisplayCount { get; set; }
        public int ItemsCount { get; set; }
    }
}
