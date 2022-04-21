using StagwellTech.SirenSDK.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

[assembly: InternalsVisibleTo("SirenTests")]
namespace StagwellTech.SirenSDK.Utils
{
    internal class YAMLLoader<T> where T : class, new()
    {

        string filePath;

        public YAMLLoader(string filePath)
        {
            this.filePath = filePath;
        }

        public T Load()
        {
            var yamlStr = TryGetStringFromFile(filePath);
            yamlStr = PreprocessYamlString(yamlStr);
            var res = DeserializeYamlToObject<T>(yamlStr);
            return res;
        }

        private static string TryGetStringFromFile(string filePath)
        {
            if (File.Exists(filePath) == false) throw new FileInaccessibleException($"File in '{filePath}' cannot be found or accessed.");

            return File.ReadAllText(filePath);
        }

        private static string PreprocessYamlString(string str)
        {
            if (str.StartsWith("siren:")) str = str.Replace("siren:", "");

            return str;
        }

        private static T DeserializeYamlToObject<T>(string yamlStr)
        {
            var deserializer = new DeserializerBuilder()
              .WithNamingConvention(CamelCaseNamingConvention.Instance)
              .IgnoreUnmatchedProperties()
              .Build();
            return deserializer.Deserialize<T>(yamlStr);
        }
    }
}
