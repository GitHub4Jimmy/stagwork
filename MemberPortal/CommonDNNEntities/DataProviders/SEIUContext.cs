using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.User;
using StagwellTech.ServiceBusRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StagwellTech.SEIU.CommonDNNEntities.DataProviders
{
    public class SEIUContext
    {
        private HttpRequest Request { get; set; }
        private int DNNUserId { get; set; }

        enum Key
        {
            USER_SETTINGS
        }

        protected MemberClient memberClient { get; set; }
        protected UserSettingsClient userSettingsClient { get; set; }

        public SEIUContext(HttpRequest Request, int DNNUserId)
        {
            var ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var rpcClient = new ServiceBusRPCClient(ServiceBusConnectionString);
            memberClient = new MemberClient(rpcClient);
            userSettingsClient = new UserSettingsClient(rpcClient);

            this.Request = Request;
            this.DNNUserId = DNNUserId;
        }

        private void Set(Key key, object value)
        {
            Request.RequestContext.HttpContext.Items.Add($"{key}_{DNNUserId}", value);
            Debug.WriteLine($"{key} was SET");
        }

        private object Get(Key key)
        {
            Debug.WriteLine($"Trying to GET {key}");
            return Request.RequestContext.HttpContext.Items[$"{key}_{DNNUserId}"];
        }

        public async Task<UserSettings> SetUserSettingsAsync()
        {
            var settings = await GetUserSettingsAsync();
            Set(Key.USER_SETTINGS, settings);
            return settings;
        }

        public async Task<UserSettings> GetUserSettingsAsync()
        {
            var settings = (UserSettings) Get(Key.USER_SETTINGS);
            if (settings == null)
            {
                settings = await userSettingsClient.getByDNNUserId(DNNUserId);
            }
            return settings;
        }

    }
}
