using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Training.Events;

namespace TrainingFund.DNN.Integration.Services
{
    public class EventService
    {
        public async Task<MPEventViewModel> Get(int personId, string id)
        {

            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                MPEventViewModel model = null;

                HttpResponseMessage response = await client.GetAsync($"event?personId={personId}&id={id}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<MPEventViewModel>();
                }

                return model;
            }
            catch (Exception e)
            {
                TelemtryLogHelper.Log(this, e);
            }

            return null;
        }

        public async Task<List<MPEventViewModel>> GetByDate(int personId, DateTime startDate, DateTime endDate)
        {

            var globalSettings = DummyContentSettingsHelper.GetSettings();

            if (globalSettings.DebugPersonId > 0)
            {
                personId = globalSettings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                List<MPEventViewModel> model = null;

                HttpResponseMessage response = await client.GetAsync($"events/by-date?personId={personId}&startDate={startDate.ToString("yyyy-MM-dd")}&endDate={endDate.ToString("yyyy-MM-dd")}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<List<MPEventViewModel>>();
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
