using System;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public class ServiceBusRPCMethodAttribute : Attribute
    {
        public ServiceBusRPCMethodAttribute(string requestName)
        {
            this.requestName = requestName;
        }

        public string requestName { get; }
    }
}