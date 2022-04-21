using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.SessionState;
using Dnn.Authentication.Auth0.Components;
using DotNetNuke.Web.Api;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using StagwellTech.SEIU.CommonEntities.User;
using TrainingFund.DNN.Integration.Services;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Training.Modals.Course;
using TrainingFund.Shared.ViewModels.Training.Modals.Enrollment;

namespace TrainingFund.DNN.Integration.Components
{
    public class CourseController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public CourseController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<MPCourseDetailsAndSessionsViewModel> Details(int courseId)
        {
            int personId = await GetUserId();

            var service = new CourseDetailService();
            var lang = TranslationHelper.GetCurrentLanguageCode();
            var result = await service.Get(courseId, personId, lang);

            return result;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<MPEnrollmentViewModel> EnrollmentDetails(int enrollmentId)
        {
            int personId = await GetUserId();

            var service = new EnrollmentDetailService();
            var lang = TranslationHelper.GetCurrentLanguageCode();
            var result = await service.Get(enrollmentId, personId, lang);

            return result;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<HttpResponseMessage> ViewCertificate(int certificateId)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var service = new CourseCertificateService();
            var personId = await GetUserId();

            BinaryFileViewModel binaryFile = await service.Get(certificateId, personId);

            if (binaryFile != null && binaryFile.Data.Length > 0)
            {
                var contentType = (binaryFile.ContentType == "pdf") ? "application/pdf" : binaryFile.ContentType;
                response.Content = new StreamContent(new MemoryStream(binaryFile.Data));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                response.Content.Headers.ContentLength = binaryFile.Data.Length;
                if (ContentDispositionHeaderValue.TryParse($"inline; filename={binaryFile.Name}", out var contentDisposition))
                {
                    response.Content.Headers.ContentDisposition = contentDisposition;
                }

                return response;
            }

            var statusCode = HttpStatusCode.NotFound;
            var message = $"Unable to find resource. Resource \"{certificateId}\" may not exist.";
            
            response = Request.CreateResponse(statusCode, new { Message = message});

            return response;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<HttpResponseMessage> DownloadTranscript()
        {

            var response = Request.CreateResponse(HttpStatusCode.OK);
            var personId = await GetUserId();
            var service = new DownloadTranscriptService();

            BinaryFileViewModel binaryFile = await service.Get(personId);

            if (binaryFile != null && binaryFile.Data.Length > 0)
            {
                var contentType = (binaryFile.ContentType == "pdf") ? "application/pdf" : binaryFile.ContentType;
                response.Content = new StreamContent(new MemoryStream(binaryFile.Data));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                response.Content.Headers.ContentLength = binaryFile.Data.Length;
                if (ContentDispositionHeaderValue.TryParse($"inline; filename={binaryFile.Name}", out var contentDisposition))
                {
                    response.Content.Headers.ContentDisposition = contentDisposition;
                }

                return response;
            }

            var statusCode = HttpStatusCode.NotFound;
            var message = $"Unable to find resource. Resource may not exist.";

            response = Request.CreateResponse(statusCode, new { Message = message });

            return response;
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Test()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, new
            {
                AccessToken = "TEST_ACCESS_TOKEN",
                OobCode = "TEST_OOB_CODE"
            });
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        private async Task<int> GetUserId()
        {
            UserSettings settings = await UserSettingsClient.getByDNNUserId(UserInfo.UserID);

            int.TryParse(settings.PersonId, out int personId);

            return personId;
        }
    }
}