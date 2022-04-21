using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.TrainingFund;

namespace TrainingFund.DNN.Integration.Helpers
{
    public class HttpClientHelper
    {
        public static HttpClient GetInstance()
        {
            //var url = (Utilities.GetWebAPIUrls().ContainsKey("SEIU_API_trainingFund")) ?  Utilities.GetWebAPIUrls()["SEIU_API_trainingFund"] : String.Empty;
            //var baseAddress = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API) ?? url;

            var baseAddress = TrainingFundHandler.GetBaseAddress();

            var id = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API_ID);
            var secret = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API_SECRET);
            var subject = Environment.GetEnvironmentVariable(TrainingFundHandler.ENVIRONMENT_TRAINING_FUND_API_SUBJECT);

            var token = TrainingFundHandler.GenerateJwtToken(id, subject, secret);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TrainingFundHandler.TOKEN_TYPE, token);
            Debug.WriteLine("JWT token => " + token);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Date = DateTime.UtcNow;

            return client;
        }
    }
}
