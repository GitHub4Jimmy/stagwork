using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using Dnn.Authentication.Auth0.Components;
using DotNetNuke.Web.Api;
using SendGrid;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.SendgridService;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using StagwellTech.SEIU.CommonEntities.User;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Services;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Components
{
    public class LocationsController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public LocationsController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<Response> SendResults()
        {
            var queryCollection = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var searchTerm = TrainingFundSearchHelper.GetSearchText(queryCollection);
            var personId = await GetUserId();

            var service = new TrainingLocationsService();
            List<FilterBoxViewModel> filters = null;
            if (TrainingFundSearchHelper.HasFilters(queryCollection))
            {
                filters = await service.GetFilters(personId);
                TrainingFundSearchHelper.SetFilterBoxes(queryCollection, filters);
            }

            var lang = TranslationHelper.GetCurrentLanguageCode();

            var result = await service.Get(personId, filters, lang);

            return await Mail.SendTrainingFundFindLocationResults(UserInfo.Email, result);

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
