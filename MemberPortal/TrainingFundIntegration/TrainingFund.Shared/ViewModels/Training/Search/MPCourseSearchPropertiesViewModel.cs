using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPCourseSearchPropertiesViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string SearchHeader { get; set; }
        [Translatable]
        public string SearchContent { get; set; }
        [Translatable]
        public string SearchContentDefault { get; set; }
        [Translatable]
        public string RefineSearchHeader { get; set; }
        [Translatable]
        public List<FilterBoxViewModel> FilterBoxes { get; set; }
    }
}
