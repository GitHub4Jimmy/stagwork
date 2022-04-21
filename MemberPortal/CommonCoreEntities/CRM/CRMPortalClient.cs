using Newtonsoft.Json;
using StagwellTech.SEIU.CommonEntities;
using StagwellTech.SEIU.CommonEntities.CRM;
using StagwellTech.ServiceBusRPC;
using StagwellTech.ServiceBusRPC.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.CRM
{
    public class CRMPortalClient<T>
    {
        private readonly ServiceBusRPCClient rpcClient;
        public CRMPortalClient(ServiceBusRPCClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public async Task<APIMessageResponse> sendMesasge(CRMMessageRequest message)
        {
            APIMessageRequest request = new APIMessageRequest();
            request.RequestId = Guid.NewGuid();
            request.RequestName = "POST";

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "data", ParameterValue = message.toJSON() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };

            var response = await rpcClient.rpcRequest("crmmessage", request.toJSON());

            return APIMessageResponse.fromJSON(Encoding.UTF8.GetString(response.Body));
        }

        public async Task<APIMessageResponse> crmDelete(Service service, string dataId, string dataType = null)
        {
            if(dataType == null)
            {
                dataType = typeof(T).FullName;
            }
            var message = CRMMessageRequest.buildDeleteMessage(CRMMessageHandlerMethodAttribute.SOURCE_PORTAL, service, dataId, dataType);
            return await sendMesasge(message);
        }

        public async Task<APIMessageResponse> crmUpdate(Service service, string data, string dataId, string dataType = null)
        {
            if (dataType == null)
            {
                dataType = typeof(T).FullName;
            }
            var message = CRMMessageRequest.buildUpdateMessage(CRMMessageHandlerMethodAttribute.SOURCE_PORTAL, service, data, dataId, dataType);
            return await sendMesasge(message);
        }

        public async Task<APIMessageResponse> crmCreate(Service service, T data)
        {
            return await crmCreate(service, JsonConvert.SerializeObject(data), typeof(T).FullName);
        }

        public async Task<APIMessageResponse> crmCreate(Service service, string data, string dataType = null)
        {
            if (dataType == null)
            {
                dataType = typeof(T).FullName;
            }
            var message = CRMMessageRequest.buildCreateMessage(CRMMessageHandlerMethodAttribute.SOURCE_PORTAL, service, data, dataType);
            return await sendMesasge(message);
        }
    }
}
