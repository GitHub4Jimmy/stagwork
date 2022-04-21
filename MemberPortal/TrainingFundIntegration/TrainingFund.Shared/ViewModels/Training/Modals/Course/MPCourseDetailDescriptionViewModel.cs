using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Course
{
    public class MPCourseDetailDescriptionViewModel
    {
        public int CourseId { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public List<MPGenericLinkButtonViewModel> Actions { get; set; }
        public List<MPGenericLinkButtonViewModel> FooterLinksSecondary { get; set; }
    }
}