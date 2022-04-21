using System;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Transcript;

namespace TrainingFund.DNN.Integration.Services
{
    public class WelcomeBoxService
    {
        public async Task<MPWelcomeBoxViewModal> Get(int personId, string targetLanguage = "en")
        {

            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPWelcomeBoxViewModal>("WelcomeBox");
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }
            
            try
            {
                var client = HttpClientHelper.GetInstance();

                MPWelcomeBoxViewModal model = null;

                HttpResponseMessage response = await client.GetAsync($"welcome-box?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPWelcomeBoxViewModal>();
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
