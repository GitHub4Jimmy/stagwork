using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.SessionState;
using Dnn.Authentication.Auth0.Components;
using DotNetNuke.Web.Api;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Docebo;
using StagwellTech.SEIU.CommonEntities.User;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Services;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Components
{
    public class DoceboCourseController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public DoceboCourseController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<HttpResponseMessage> Launch(string url)
        {
            var service = new DoceboUsernameService();
            var personId = await GetUserId();
            var username = await service.Get(personId);
            var redirectUrl = DotNetNuke.Common.Globals.NavigateURL(DotNetNuke.Common.Globals.GetPortalSettings().ErrorPage404);

            if (!String.IsNullOrEmpty(username))
            {
                var signedUrl = DoceboJwtHandler.SignRedirectURL(url, username);
                if (!String.IsNullOrEmpty(signedUrl))
                {
                    redirectUrl = signedUrl;
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(redirectUrl);

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
