using System;

namespace StagwellTech.SirenSDK.Exceptions
{
    public class ConfigInvalidException : Exception
    {
        public ConfigInvalidException(string message) : base(message) { }
    }
}
