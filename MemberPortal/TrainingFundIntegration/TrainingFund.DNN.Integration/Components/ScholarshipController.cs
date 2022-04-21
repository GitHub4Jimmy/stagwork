using Dnn.Authentication.Auth0.Components;
using DotNetNuke.Web.Api;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.User;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.SessionState;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.DNN.Integration.Services;
using TrainingFund.Shared.Constants;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Scholarship;

namespace TrainingFund.DNN.Integration.Components
{
    public class ScholarshipController : DnnApiController, IRequiresSessionState
    {
        protected Auth0Client Client { get; set; }
        protected UserSettingsClient UserSettingsClient { get; set; }

        public ScholarshipController()
        {
            Client = new Auth0Client(PortalSettings.PortalId);
            UserSettingsClient = UserSettingsClient.Instance;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<HttpResponseMessage> DeleteApplication(int applicationId)
        {
            var personId = await GetUserId();

            var service = new ScholarshipService();

            var result = service.DeleteApplication(personId, applicationId);

            var redirectUrl = LinkHelper.GetDnnUrl(KeyIdentifiers.PAGE_SCHOLARSHIP_MAIN_KEY);
            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(redirectUrl);

            return response;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<HttpResponseMessage> ViewEssay(int linkedUploadId)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var service = new DownloadScholarshipEssayService();
            var personId = await GetUserId();

            BinaryFileViewModel binaryFile = await service.Get(linkedUploadId);

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
            var message = $"Unable to find resource. Resource \"{linkedUploadId}\" may not exist.";

            response = Request.CreateResponse(statusCode, new { Message = message });

            return response;
        }

        [DnnAuthorize(StaticRoles = "Registered Users")]
        [HttpGet]
        public async Task<MPStatusModalViewModel> Status(int applicationId)
        {
            int personId = await GetUserId();

            var service = new ScholarshipService();
            var result = await service.GetApplicationStatus(personId, applicationId);

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
