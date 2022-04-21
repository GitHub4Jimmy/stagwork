using StagwellTech.SirenSDK.Configs;
using StagwellTech.SirenSDK.Providers;
using StagwellTech.SirenSDK.Utils;
using System;
using System.Net.Http.Headers;

namespace StagwellTech.SirenSDK
{

    public class Siren
    {
        private static Siren instance = null;

        public BaseProviderClient Provider { get; }

        private SirenConfig Config { get; }

        public static Siren GetInstance(string configPath)
        {
            if (instance == null) instance = new Siren(configPath);
            return instance;
        }

        public static Siren GetInstance(SirenConfig config)
        {
            if (instance == null) instance = new Siren(config);
            return instance;
        }

        private Siren(string configPath)
        {
            Config = LoadConfig(configPath);
            Provider = InitializeProvider(Config);
        }

        private Siren(SirenConfig config)
        {
            Config = config;
            Provider = InitializeProvider(Config);
        }

        private static SirenConfig LoadConfig(string configPath)
        {
            var loader = new YAMLLoader<SirenConfig>(configPath);
            return loader.Load();
        }

        private static BaseProviderClient InitializeProvider(SirenConfig config)
        {
            return ProviderClientFactory.CreateClient(config);
        }

    }
}
