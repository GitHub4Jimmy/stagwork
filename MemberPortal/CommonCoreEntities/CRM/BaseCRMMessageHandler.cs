using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.CRM;
using StagwellTech.ServiceBusRPC;

namespace StagwellTech.SEIU.CommonCoreEntities.CRM
{
    public abstract class BaseCRMMessageHandler : ICRMMessageHandler
    {
        protected readonly ServiceBusRPCClient rpcClient;

        public BaseCRMMessageHandler(ServiceBusRPCClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public async Task<CRMMessageResponse> handleMessage(CRMMessageRequest message)
        {
            var requestName = message.RequestName.ToUpper(); //CREATE, DELETE, UPDATE...
            var source = message.Source;

            foreach (var method in GetType().GetMethods())
            {

                CRMMessageHandlerMethodAttribute serviceBusMethodAttr = (CRMMessageHandlerMethodAttribute)method.GetCustomAttribute(typeof(CRMMessageHandlerMethodAttribute));

                if (serviceBusMethodAttr != null && serviceBusMethodAttr.requestName == requestName && serviceBusMethodAttr.source == source)
                {
                    try
                    {
                        return await (Task<CRMMessageResponse>)method.Invoke(this, new[] { message });
                    }
                    catch (Exception e)
                    {
                        return CRMMessageResponse.build500Response(message, e.Message);
                    }
                }

            }

            return CRMMessageResponse.build404Response(message, null);

        }

        public abstract Task<CRMMessageResponse> crmCreate(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> crmCreateOrUpdate(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> crmDelete(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> crmUpdate(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> portalCreate(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> portalCreateOrUpdate(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> portalDelete(CRMMessageRequest message);
        public abstract Task<CRMMessageResponse> portalUpdate(CRMMessageRequest message);
    }
}
