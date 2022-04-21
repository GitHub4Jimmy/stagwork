using System;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Modals.Course;
using TrainingFund.Shared.ViewModels.Training.Modals.Session;

namespace TrainingFund.DNN.Integration.Services
{
    public class CourseDetailService
    {
        public async Task<MPCourseDetailsAndSessionsViewModel> Get(int courseId, int personId, string targetLanguage = "en")
        {
            var settings = DummyContentSettingsHelper.GetSettings();

            if (settings.UseDebugContent)
            {
                return CreateDebugCourseContent();
            }

            if (settings.DebugPersonId > 0)
            {
                personId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPCourseDetailsAndSessionsViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"course-detail?personId={personId}&courseId={courseId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPCourseDetailsAndSessionsViewModel>();
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

        private MPCourseDetailsAndSessionsViewModel CreateDebugCourseContent()
        {
            return new MPCourseDetailsAndSessionsViewModel()
            {
                CourseDetails = DummyContentHelper.Load<MPCourseDetailsViewModel>("CourseSearchResultsDetail"),
                Sessions = DummyContentHelper.Load<MPSessionsBoxViewModel>("CourseSearchResultsDetailSessions")
            };
        }

    }
}
