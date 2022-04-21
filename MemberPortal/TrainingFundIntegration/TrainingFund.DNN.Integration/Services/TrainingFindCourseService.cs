using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using TrainingFund.DNN.Integration.Extensions;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Training.Search;

namespace TrainingFund.DNN.Integration.Services
{
    public class TrainingFindCourseService
    {
        public async Task<MPCourseSearchViewModel> Get(int personId, string searchTerm, List<FilterBoxViewModel> filters, string targetLanguage = "en")
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return new MPCourseSearchViewModel()
                {
                    Properties = DummyContentHelper.Load<MPCourseSearchPropertiesViewModel>("CourseSearch"),
                    Results = DummyContentHelper.Load<MPCourseSearchResultsBoxViewModel>("CourseSearchResults")
                };
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPCourseSearchViewModel model = null;

                var data = new SearchViewModel()
                {
                    PersonId = personId,
                    SearchTerm = searchTerm,
                    Filters = filters
                };

                HttpResponseMessage response = await client.PostAsJsonAsync($"find-a-course", data);

                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPCourseSearchViewModel>();
                }

                if (model != null && targetLanguage != TrainingFundHandler.TRAINGFUND_BASE_LANGUAGE_CODE)
                {
                    var trans = model.GetTranslatable();

                    TranslationHelper.FromObjectDescriptor(trans,
                        TrainingFundHandler.TRAINGFUND_BASE_LANGUAGE_CODE, targetLanguage);

                }

                model.ProcessLinks();

                return model;
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return null;

        }

        public async Task<List<FilterBoxViewModel>> GetFilters(int personId)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return DummyContentHelper.Load<MPCourseSearchPropertiesViewModel>("CourseSearch").FilterBoxes;
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                List<FilterBoxViewModel> model = null;

                HttpResponseMessage response = await client.GetAsync($"find-a-course-filters?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<List<FilterBoxViewModel>>();
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
