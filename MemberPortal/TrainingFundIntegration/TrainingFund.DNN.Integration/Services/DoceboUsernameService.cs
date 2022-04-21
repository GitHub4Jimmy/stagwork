using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.DNN.Integration.Helpers;
using TrainingFund.Shared.ViewModels.Scholarship;

namespace TrainingFund.DNN.Integration.Services
{
    public class DoceboUsernameService
    {

        public async Task<string> Get(int personId)
        {
            var settings = DummyContentSettingsHelper.GetSettings();

            if (!String.IsNullOrEmpty(settings.DoceboDebugUsername))
            {
                return settings.DoceboDebugUsername;
            }

            if (settings.DebugPersonId > 0)
            {
                personId = settings.DebugPersonId;
            }

            try
            {
                var client = HttpClientHelper.GetInstance();

                string model = null;

                HttpResponseMessage response = await client.GetAsync($"get-docebo-username?personId={personId}");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<string>();
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
