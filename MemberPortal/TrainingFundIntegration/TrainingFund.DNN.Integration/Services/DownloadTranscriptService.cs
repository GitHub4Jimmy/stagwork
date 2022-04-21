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
    public class DownloadTranscriptService
    {
        public async Task<BinaryFileViewModel> Get(int personId)
        {
            var settings = DummyContentSettingsHelper.GetSettings();

            if (settings.UseDebugContent)
            {
                return CreateDebugDownloadTranscriptContent();
            }

            if (settings.DebugPersonId > 0)
            {
                personId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                BinaryFileViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"transcript-download?personId={personId}");

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



        private BinaryFileViewModel CreateDebugDownloadTranscriptContent()
        {
            return new BinaryFileViewModel()
            {
                Name = "Course_Transcript.pdf",
                Data = DummyContentHelper.LoadBinary("DownloadTranscript"),
                ContentType = "application/pdf"
            };
        }
    }
}
