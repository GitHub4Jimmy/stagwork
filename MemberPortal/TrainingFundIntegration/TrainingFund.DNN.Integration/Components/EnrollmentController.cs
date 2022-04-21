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
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Services;
using TrainingFund.Shared.ViewModels.Training.Modals.Enrollment;

namespace TrainingFund.DNN.Integration.Components
{
    public class EnrollmentController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public EnrollmentController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<MPEnrollmentResultViewModel> Set(int courseId, int sessionId = 0)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPEnrollmentResultViewModel>("CourseUnenroll");
            }

            var service = new EnrollmentService();
            int personId = await GetUserId();

            var lang = TranslationHelper.GetCurrentLanguageCode();

            var model = await service.Set(personId, courseId, sessionId, lang);

            model?.ProcessLinks();

            return model;
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
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            UserSettings settings = await UserSettingsClient.getByDNNUserId(UserInfo.UserID);

            if (int.TryParse(settings.PersonId, out int personId))
            {
                if (globalSettings.DebugPersonId > 0)
                {
                    personId = globalSettings.DebugPersonId;
                }

            }

            return personId;
        }
    }
}
