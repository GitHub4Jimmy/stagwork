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
using TrainingFund.Shared.ViewModels.Training.Modals.Course;

namespace TrainingFund.DNN.Integration.Components
{
    public class CareerTrackController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public CareerTrackController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
        }

        
        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<MPCourseDetailsViewModel> Details(int courseId)
        {
            int personId = await GetUserId();
            var service = new CareerTrackService();
            var lang = TranslationHelper.GetCurrentLanguageCode();

            var result = await service.Get(courseId, personId, lang);

            return result;
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
