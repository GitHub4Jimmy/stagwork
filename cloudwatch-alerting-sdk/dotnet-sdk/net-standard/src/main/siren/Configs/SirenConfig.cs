using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet;
using YamlDotNet.Serialization;

namespace StagwellTech.SirenSDK.Configs
{
    public class SirenConfig
    {
        public class AzureApplicationInsights
        {
            public string instrumentationKey;
            public string appId;
            public string apiKey;
            //public string profile;
            //public string accessKey;
            //public string secretKey;
            //public int httpConnectionsCount;
        }



        public AzureApplicationInsights applicationInsights;
        public bool enabled = true;
        public bool developerMode = false;
        //public string provider;
        //public bool async = true;
        //public int asyncWaitTime = 500;

    }
}
