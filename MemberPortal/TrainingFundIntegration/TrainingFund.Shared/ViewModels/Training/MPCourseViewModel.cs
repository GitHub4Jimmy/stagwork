using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training
{
    public class MPCourseViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }

        public int CourseId { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel Name { get; set; }
        [Translatable]
        public string Delivery { get; set; }
        [Translatable]
        public string Description { get; set; }
        public string CourseImageJPG { get; set; }

        public bool isVisibleAvailable { get; set; }
        [Translatable]
        public string AvailableText { get; set; }

        public bool isVisibleFlair { get; set; }
        [Translatable]
        public string FlairText { get; set; }

        public bool isVisibleEstimatedTime { get; set; }

        [Translatable]
        public string EstimatedTimeHeader { get; set; }
        public string EstimatedTime { get; set; }

        public bool isVisibleYourProgress { get; set; }
        [Translatable]
        public string YourProgressHeader { get; set; }
        [Translatable]
        public string YourProgress { get; set; }
    }
}