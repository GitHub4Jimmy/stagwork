using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Search
{
    public class MPCourseSearchResultViewModel : ITranslatable
    {
        public int CourseId { get; set; }
        public int V3PersonId { get; set; }
        public int TrainingFundUserId { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string NameDisplay { get; set; }
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
        [Translatable]
        public MPGenericLinkButtonViewModel Name { get; set; }
    }
}