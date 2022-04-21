using System;

namespace StagwellTech.SirenSDK.Exceptions
{
    public class FileInaccessibleException : Exception
    {
        public FileInaccessibleException(string message) : base(message) { }
    }
}
