using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPCourseSearchViewModel : ITranslatable
    {
        [Translatable]
        public MPCourseSearchPropertiesViewModel Properties { get; set; }
        [Translatable]
        public MPCourseSearchResultsBoxViewModel Results { get; set; }
    }
}
