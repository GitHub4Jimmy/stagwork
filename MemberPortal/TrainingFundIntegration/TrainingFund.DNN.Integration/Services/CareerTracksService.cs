using System;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Search;

namespace TrainingFund.DNN.Integration.Services
{
    public class CareerTracksService
    {
        public async Task<MPCareerTrackSearchViewModel> Get(int personId, string targetLanguage = "en")
        {

            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPCareerTrackSearchViewModel>("CareerTrackSearch");
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }
            
            try
            {
                var client = HttpClientHelper.GetInstance();

                MPCareerTrackSearchViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"career-tracks?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPCareerTrackSearchViewModel>();
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
