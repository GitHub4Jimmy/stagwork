using System;

namespace StagwellTech.SirenSDK.Exceptions
{
    public class MissingCredentialsException : Exception
    {
        public MissingCredentialsException(string message) : base(message) { }
    }
}
