using Newtonsoft.Json;
using StagwellTech.SirenSDK.Configs;
using StagwellTech.SirenSDK.Exceptions;
using StagwellTech.SirenSDK.Utils;
using StagwellTech.SirenTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Xunit;

namespace StagwellTech.SirenTests
{
    public class YAMLLoaderTests
    {

        [Fact]
        public void ConstructYAMLLoader()
        {
            string filePath = "some path";
            var loader = new YAMLLoader<SirenConfig>(filePath);
            Assert.NotNull(loader);
        }

        [Theory]
        [InlineData("loadConfigFromPathFirstLineSiren.yaml")]
        [InlineData("loadConfigFromPathMissingFirstLine.yaml")]
        [InlineData("loadConfigFromPathMissingNodes.yaml")]
        [InlineData("loadConfigFromPathTooManyNodes.yaml")]
        public void LoadYAMLConfigFromPath(string fileName)
        {
            string filePath = $"{TestUtils.ResourcesDirectory}{fileName}";
            var loader = new YAMLLoader<SirenConfig>(filePath);

            SirenConfig config = loader.Load();

            var expectedConfig = new SirenConfig
            {
                enabled = true,
                developerMode = true,
                applicationInsights = new SirenConfig.AzureApplicationInsights
                {
                    instrumentationKey = "dummyInstrumentationKey",
                    appId = "dummyId",
                    apiKey = "dummyAPIKey"
                }
            };

            Assert.NotNull(config);
            var actualJson = JsonConvert.SerializeObject(config);
            var expectedJson = JsonConvert.SerializeObject(expectedConfig);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void InvalidFilePathThrowsException()
        {
            string filePath = "Invalid path name";
            var loader = new YAMLLoader<SirenConfig>(filePath);

            Assert.Throws<FileInaccessibleException>(() => loader.Load());
        }

    }
}
