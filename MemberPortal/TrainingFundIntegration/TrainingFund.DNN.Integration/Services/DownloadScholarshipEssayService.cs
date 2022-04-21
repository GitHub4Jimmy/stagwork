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
    public class DownloadScholarshipEssayService
    {
        public async Task<BinaryFileViewModel> Get(int linkedUploadId)
        {
            var settings = DummyContentSettingsHelper.GetSettings();
            
            if (settings.DebugPersonId > 0)
            {
                linkedUploadId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                BinaryFileViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"scholarship-application-document?linkedUploadId={linkedUploadId}");

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

    }
}
