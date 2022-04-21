using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels;

namespace TrainingFund.DNN.Integration.Services
{
    public class CourseCertificateService
    {

        public async Task<BinaryFileViewModel> Get(int certificateId, int personId)
        {
            var settings = DummyContentSettingsHelper.GetSettings();

            if (settings.UseDebugContent)
            {
                return CreateDebugViewCertificateContent();
            }

            if (settings.DebugPersonId > 0)
            {
                personId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                BinaryFileViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"course-certificate?personId={personId}&enrollmentId={certificateId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<BinaryFileViewModel>();
                }

                return model;
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return null;
        }

        private BinaryFileViewModel CreateDebugViewCertificateContent()
        {

            return new BinaryFileViewModel()
            {
                Name = "Sample_Certificate.pdf",
                Data = DummyContentHelper.LoadBinary("ViewCertificate"),
                ContentType = "application/pdf"
            };
        }
    }
}
