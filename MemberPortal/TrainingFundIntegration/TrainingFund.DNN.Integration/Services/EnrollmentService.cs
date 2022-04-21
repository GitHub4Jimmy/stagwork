using System;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Training.Modals.Enrollment;

namespace TrainingFund.DNN.Integration.Services
{
    public class EnrollmentService
    {
        public async Task<MPEnrollmentResultViewModel> Set(int personId, int courseId, int sessionId = 0, string targetLanguage = "en")
        {
            try
            {
                var client = HttpClientHelper.GetInstance();
                var param = new EnrollmentViewModel()
                {
                    PersonId = personId,
                    CourseId = courseId,
                    SessionId = sessionId
                };

                HttpResponseMessage response = await client.PostAsJsonAsync(
                    $"enrollment",  param);

                MPEnrollmentResultViewModel model = null;

                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPEnrollmentResultViewModel>();
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
