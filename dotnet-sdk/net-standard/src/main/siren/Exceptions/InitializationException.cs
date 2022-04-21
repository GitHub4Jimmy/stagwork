using System;

namespace StagwellTech.SirenSDK.Exceptions
{
    public class InitializationException : Exception
    {
        public InitializationException(string message) : base(message) { }
    }
}
