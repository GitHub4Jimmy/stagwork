using System;

namespace StagwellTech.SirenSDK.Exceptions
{
    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException(string message) : base(message) { }
    }
}
