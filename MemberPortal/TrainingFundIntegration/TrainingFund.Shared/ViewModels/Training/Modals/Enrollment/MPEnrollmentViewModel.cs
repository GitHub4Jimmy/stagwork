using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;
using TrainingFund.Shared.ViewModels.Training.Modals.Course;
using TrainingFund.Shared.ViewModels.Training.Modals.Session;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Enrollment
{
    public class MPEnrollmentViewModel : ITranslatable
    {
        [Translatable]
        public string CourseName { get; set; }
        [Translatable]
        public MPEnrollmentStatusViewModel StatusDetails { get; set; }
        [Translatable]
        public MPSessionNextClassViewModel NextClass { get; set; }
        [Translatable]
        public MPCourseDescriptionViewModel CourseDescriptionDetails { get; set; }
        [Translatable]
        public MPSessionInstructorViewModel InstructorDetails { get; set; }
        [Translatable]
        public MPSessionLocationViewModel LocationDetails { get; set; }
        [Translatable]
        public MPSessionScheduleViewModel ClassSchedule { get; set; }
        public bool isVisibleCareerTrack { get; set; }
        [Translatable]
        public MPCourseViewModel CareerTrack { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> FooterLinks { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> FooterLinksSecondary { get; set; }


        #region deprecated

        public bool isStatusCheckMark;
        public string Status;
        public string StatusHeader;

        public string CourseDescription { get; set; }

        public List<MPSessionInstructorItemViewModel> Instructors { get; set; }
        public string InstructorHeader { get; set; }
        public bool isVisibleInstructor { get; set; }

        public string Times { get; set; }
        public string Dates { get; set; }
        public string DateAndTimeHeader { get; set; }
        public bool isVisibleDateAndTime { get; set; }

        public string RoomName { get; set; }
        public string RoomHeader { get; set; }
        public int RoomId { get; set; }
        public bool isVisibleRoom { get; set; }

        public MPGenericLinkButtonViewModel LocationLink { get; set; }
        public string LocationAddress { get; set; }
        public string LocationName { get; set; }
        public string LocationHeader { get; set; }
        public int LocationId { get; set; }
        public bool isVisibleLocation { get; set; }

        #endregion
    }
}
