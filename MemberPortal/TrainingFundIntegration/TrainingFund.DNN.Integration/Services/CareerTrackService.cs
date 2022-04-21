using System;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Modals.Course;

namespace TrainingFund.DNN.Integration.Services
{
    public class CareerTrackService
    {
        public async Task<MPCourseDetailsViewModel> Get(int courseId, int personId, string targetLanguage = "en")
        {
            var settings = DummyContentSettingsHelper.GetSettings();

            if (settings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPCourseDetailsViewModel>("CareerTrackSearchDetail");
            }

            if (settings.DebugPersonId > 0)
            {
                personId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPCourseDetailsViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"career-track?personId={personId}&courseId={courseId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPCourseDetailsViewModel>();
                }

                if (model != null && targetLanguage != TrainingFundHandler.TRAINGFUND_BASE_LANGUAGE_CODE)
                {
                    var trans = model.GetTranslatable();

                    TranslationHelper.FromObjectDescriptor(trans,
                        TrainingFundHandler.TRAINGFUND_BASE_LANGUAGE_CODE, targetLanguage);

                }

                model?.ProcessLinks();

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
