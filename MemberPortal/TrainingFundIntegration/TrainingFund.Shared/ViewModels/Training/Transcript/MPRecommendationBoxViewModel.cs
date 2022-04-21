using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPRecommendationBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }

        [Translatable]
        public List<MPCourseViewModel> Recommendations { get; set; }
        public bool isVisible { get; set; }
    }
}