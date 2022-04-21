using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;
using TrainingFund.Shared.ViewModels.Training.Transcript;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPLocationSearchViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string RefineSearchHeader { get; set; }
        [Translatable]
        public List<FilterBoxViewModel> FilterBoxes { get; set; }
        [Translatable]
        public string ResultsHeader { get; set; }
        [Translatable]
        public List<MPLocationViewModel> Locations { get; set; }
        public int DisplayCount { get; set; }
        public int LocationCount { get; set; }
    }
}