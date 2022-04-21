using System;

namespace StagwellTech.SirenSDK.Exceptions
{
    public class MetricException : Exception
    {
        public MetricException(string message) : base(message) { }
    }
}
