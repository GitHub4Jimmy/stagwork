using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Scholarship;

namespace TrainingFund.DNN.Integration.Services
{
    public class ScholarshipService
    {
        public async Task<ScholarshipCycleBoxesViewModel> GetApplications(int personId)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings(); 

            if (globalSettings.UseDebugContent)
            {
               return new ScholarshipCycleBoxesViewModel()
               {
                   CycleGridView = DummyContentHelper.Load<MPCycleGridViewModel>("ScholarshipCycleGrid"),
                   CycleBox = DummyContentHelper.Load<MPCycleBoxViewModel>("ScholarshipCycleBox")
               };
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                ScholarshipCycleBoxesViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"scholarships?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<ScholarshipCycleBoxesViewModel>();
                }

                return model;
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return null;

        }

        public async Task<MPApplicationFormViewModel> GetApplicationStep(int personId, int applicationId, int step = 0)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();
            
            if (globalSettings.UseDebugContent)
            {
                if (step <= 0)
                {
                    step = 1;
                }

                var key = $"YourApplication_{step}";
                return DummyContentHelper.Load<MPApplicationFormViewModel>(key);
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPApplicationFormViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"scholarship-application?personId={personId}&applicationId={applicationId}&step={step}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPApplicationFormViewModel>();
                }

                return model;
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return null;
        }

        public async Task<MPApplicationFormSubmitViewModel> SaveApplicationStep(int personId, int applicationId, int step, List<MPSubmissionDataViewModel> responses)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return new MPApplicationFormSubmitViewModel();
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                HttpResponseMessage response = await client.PostAsJsonAsync(
                    $"scholarship-application?personId={personId}&applicationId={applicationId}&step={step}", responses);

                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<MPApplicationFormSubmitViewModel>();
                }
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return null;
        }

        public async Task<bool> DeleteApplication(int personId, int applicationId)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return true;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                HttpResponseMessage response = await client.DeleteAsync(
                    $"scholarship-application?personId={personId}&applicationId={applicationId}");

                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return false;
        }

        public async Task<MPStatusModalViewModel> GetApplicationStatus(int personId, int applicationId)
        {
            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPStatusModalViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"scholarship-application-status?personId={personId}&applicationId={applicationId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPStatusModalViewModel>();
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
