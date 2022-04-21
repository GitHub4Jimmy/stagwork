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

namespace TrainingFund.DNN.Integration.Components
{
    public class TestController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public TestController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
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
    }
}
