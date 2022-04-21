using System;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Modals.Enrollment;

namespace TrainingFund.DNN.Integration.Services
{
    public class EnrollmentDetailService
    {
        public async Task<MPEnrollmentViewModel> Get(int enrollmentId, int personId, string targetLanguage = "en")
        {
            var settings = DummyContentSettingsHelper.GetSettings();

            if (settings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPEnrollmentViewModel>("YourEnrolledCourses");
            }

            if (settings.DebugPersonId > 0)
            {
                personId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPEnrollmentViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"enrollment-detail?personId={personId}&enrollmentId={enrollmentId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPEnrollmentViewModel>();
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
