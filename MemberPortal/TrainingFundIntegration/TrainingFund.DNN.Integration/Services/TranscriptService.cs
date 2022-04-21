using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Transcript;

namespace TrainingFund.DNN.Integration.Services
{
    public class TranscriptService
    {
        public async Task<MPTranscriptPanelViewModel> Get(int personId, string targetLanguage = "en")
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPTranscriptPanelViewModel>("YourCourses");
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPTranscriptPanelViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"transcript?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPTranscriptPanelViewModel>();
                } else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("TRAINING COURSES content => " + content);
                }

                if (model != null && targetLanguage != TrainingFundHandler.TRAINGFUND_BASE_LANGUAGE_CODE)
                {
                    var trans = model.GetTranslatable();

                    TranslationHelper.FromObjectDescriptor(trans,
                        TrainingFundHandler.TRAINGFUND_BASE_LANGUAGE_CODE, targetLanguage);

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
