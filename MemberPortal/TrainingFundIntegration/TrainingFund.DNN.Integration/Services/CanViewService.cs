using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels;
using TrainingFund.Shared.ViewModels.Training;

namespace TrainingFund.DNN.Integration.Services
{
    public class CanViewService
    {
        public async Task<CanViewViewModel> Get(int personId)
        {

            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.UseDebugContent)
            {
                return new CanViewViewModel()
                {
                    IsTrainingEnrollButtonVisible = true,
                    //IsTrainingCardVisible = true,
                    //IsScholarshipCardVisible = true,
                    IsScholarshipMenuVisible = true,
                    IsTrainingMenuVisible = true
                };
            }

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = Task.Run(() => HttpClientHelper.GetInstance()).Result;

                CanViewViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"can-view?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<CanViewViewModel>();
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
