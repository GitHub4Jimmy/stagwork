using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;
using TrainingFund.Shared.ViewModels.Training.Modals.CareerTrack;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Course
{
    public class MPCourseDetailsViewModel : ITranslatable
    {
        public int CourseId { get; set; }
        [Translatable]
        public string Name { get; set; }
        [Translatable]
        public string Description { get; set; }

        public bool isVisibleFlair { get; set; }
        [Translatable]
        public string FlairText { get; set; }

        public bool isVisibleCourseHours { get; set; }
        [Translatable]
        public string CourseHoursHeader { get; set; }
        public string CourseHours { get; set; }

        public bool isVisibleCourseType { get; set; }
        [Translatable]
        public string CourseTypeHeader { get; set; }
        [Translatable]
        public string CourseType { get; set; }

        public bool isVisiblePrereqs { get; set; }
        [Translatable]
        public string PrereqsHeader { get; set; }
        [Translatable]
        public List<MPCoursePrereqBoxViewModel> PrereqBoxes { get; set; }

        public bool isVisibleCareerTrack { get; set; }
        [Translatable]
        public string CareerTrackHeader { get; set; }
        [Translatable]
        public List<MPCareerTrackGroupViewModel> CareerTrackGroups { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> Actions { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> FooterLinksSecondary { get; set; }
    }
}