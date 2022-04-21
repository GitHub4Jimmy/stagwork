using System.Linq;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Training;
using TrainingFund.Shared.ViewModels.Training.Modals.CareerTrack;
using TrainingFund.Shared.ViewModels.Training.Modals.Course;
using TrainingFund.Shared.ViewModels.Training.Modals.Enrollment;
using TrainingFund.Shared.ViewModels.Training.Modals.Session;
using TrainingFund.Shared.ViewModels.Training.Search;
using TrainingFund.Shared.ViewModels.Training.Transcript;

namespace TrainingFund.DNN.Integration.Extensions
{
    public static class ProcessLinksExtensions
    {
        public static void ProcessLinks(this MPGenericLinkButtonViewModel obj)
        {
            var linkAttributes = LinkHelper.GetAttributes(obj);
            var url = linkAttributes.Url;

            // Add attribute for modals
            if ((obj.DatabaseName == LinkIdentifiers.MODAL_ENROLLED 
                 || obj.DatabaseName == LinkIdentifiers.MODAL_COURSE
                 || obj.DatabaseName == LinkIdentifiers.MODAL_CAREER_TRACK
                 || obj.DatabaseName == LinkIdentifiers.MODAL_SESSIONS
                 || obj.DatabaseName == LinkIdentifiers.ENROLL_COURSE) 
                && linkAttributes.DataAttributes != null
                && linkAttributes.DataAttributes.Count > 0)
            {
                var pair = linkAttributes.DataAttributes.FirstOrDefault();
                var id = pair.Value;
                var attribute = pair.Key;

                obj.DatabaseIds.Add("Id", id);
                obj.DatabaseIds.Add("Attribute", attribute);
            }

            obj.Link = url;
        }

        public static void ProcessLinks(this MPCourseSearchViewModel obj)
        {
            obj.Results?.ProcessLinks();
        }
        
        public static void ProcessLinks(this MPCourseSearchResultsBoxViewModel obj)
        {
            obj.Items?.ForEach(l => l.ProcessLinks());
        }
        
        public static void ProcessLinks(this MPCourseSearchResultViewModel obj)
        {
            obj.Name?.ProcessLinks();
        }

        public static void ProcessLinks(this MPLocationSearchViewModel obj)
        {
            obj.Locations?.ForEach(l => l.ProcessLinks());
        }
        
        public static void ProcessLinks(this MPLocationViewModel obj)
        {
            obj.DirectionsLink?.ProcessLinks();
            obj.SearchLink?.ProcessLinks();
        }

        public static void ProcessLinks(this MPCourseDetailsAndSessionsViewModel obj)
        {
            obj.CourseDetails?.ProcessLinks();
            obj.Sessions?.ProcessLinks();
        }

        public static void ProcessLinks(this MPCourseDetailsViewModel obj)
        {
            obj.Actions?.ForEach(a => a.ProcessLinks());
            obj.FooterLinksSecondary?.ForEach(a => a.ProcessLinks());
            obj.CareerTrackGroups?.ForEach(ct => ct.ProcessLinks());
            obj.PrereqBoxes?.ForEach(p => p.ProcessLinks());
        }

        public static void ProcessLinks(this MPCoursePrereqBoxViewModel obj)
        {
            obj.Prereqs?.ForEach(p => p.ProcessLinks());
        }

        public static void ProcessLinks(this MPCoursePrereqViewModel obj)
        {
            obj.CourseName?.ProcessLinks();
        }

        public static void ProcessLinks(this MPSessionsBoxViewModel obj)
        {
            obj.Sessions?.ForEach(s => s.ProcessLinks());
        }

        public static void ProcessLinks(this MPSessionViewModel obj)
        {
            obj.Actions?.ForEach(o => o.ProcessLinks());
        }

        public static void ProcessLinks(this MPCourseViewModel obj)
        {
            obj.Name?.ProcessLinks();
        }
        public static void ProcessLinks(this MPCareerTrackGroupViewModel obj)
        {
            obj.Courses?.ForEach(o => o.ProcessLinks());
        }

        public static void ProcessLinks(this MPCareerTrackCourseViewModel obj)
        {
            obj.CourseName.ProcessLinks();
            obj.Actions?.ForEach(a => a.ProcessLinks());
        }

        public static void ProcessLinks(this MPEnrollmentViewModel obj)
        {
            obj.CareerTrack?.ProcessLinks();
            obj.LocationLink?.ProcessLinks();
            obj.FooterLinks?.ForEach(o => o.ProcessLinks());
            obj.FooterLinksSecondary?.ForEach(o => o.ProcessLinks());
            obj.NextClass?.ProcessLinks();
            obj.LocationDetails?.ProcessLinks();
        }

        public static void ProcessLinks(this MPSessionNextClassViewModel obj)
        {
            obj.LocationLink?.ProcessLinks();
            obj.FooterLinks?.ForEach(o => o.ProcessLinks());
        }

        public static void ProcessLinks(this MPSessionLocationViewModel obj)
        {
            obj.Items?.ForEach(o => o.ProcessLinks());
        }

        public static void ProcessLinks(this MPSessionLocationItemViewModel obj)
        {
            obj.Link?.ProcessLinks();
        }

        public static void ProcessLinks(this MPEnrollmentResultViewModel obj)
        {
            obj.CourseDetails?.ProcessLinks();
            obj.Recommendations?.ProcessLinks();

        }

        public static void ProcessLinks(this MPEnrollmentResultCourseBoxViewModel obj)
        {
            obj.Actions?.ForEach(o => o.ProcessLinks());
        }

        public static void ProcessLinks(this MPRecommendationBoxViewModel obj)
        {
            obj.Recommendations?.ForEach(o => o.ProcessLinks());
        }
    }
}
