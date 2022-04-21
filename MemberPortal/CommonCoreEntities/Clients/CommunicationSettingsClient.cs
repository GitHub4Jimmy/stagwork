using Newtonsoft.Json;
using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using StagwellTech.SEIU.CommonEntities;
using StagwellTech.SEIU.CommonEntities.User;
using StagwellTech.ServiceBusRPC;
using StagwellTech.ServiceBusRPC.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Clients
{
    public class CommunicationSettingsClient
    {
        private readonly ServiceBusRPCClient rpcClient;
        public CommunicationSettingsClient(ServiceBusRPCClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public async Task<APIMessageResponse> sendMesasge(APIMessageRequest request)
        {
            var responseMessage = await rpcClient.rpcRequest(Service.COMMUNICATION_SETTING.ToString(), request.toJSON());
            var response = APIMessageResponse.fromJSON(Encoding.UTF8.GetString(responseMessage.Body));

            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return response;
            }
            else if (response.Status == APIMessageResponse.RESPONSE_400)
            {
                throw new ArgumentException(response.Response == null ? "Request failed" : JsonConvert.SerializeObject(response.Response));
            }
            else if (response.Status == APIMessageResponse.RESPONSE_401)
            {
                throw new UnauthorizedAccessException(response.Response == null ? "Unauthorized failed" : JsonConvert.SerializeObject(response.Response));
            }
            else if (response.Status == APIMessageResponse.RESPONSE_404)
            {
                throw new Exception(response.Response == null ? "Not found" : JsonConvert.SerializeObject(response.Response));
            }
            else
            {
                throw new Exception(response.Response == null ? "Unknown error" : JsonConvert.SerializeObject(response.Response));
            }
        }

        public async Task<CommunicationSetting> Post(CommunicationSetting setting, string source = null)
        {
            APIMessageRequest request = new APIMessageRequest() {
                RequestId = new Guid(),
                RequestName = "POST",
                Source = source == null ? "PORTAL" : source
            };

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "data", ParameterValue = setting.toJSON() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };

            var response = await sendMesasge(request);

            if(response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return CommunicationSetting.fromJSON((string)response.Response);
            }

            throw new Exception("Can not create communication setting");
        }
        
        public async Task<IList<CommunicationSetting>> GetAll()
        {
            APIMessageRequest request = new APIMessageRequest()
            {
                RequestId = new Guid(),
                RequestName = "GETALL"
            };

            var response = await sendMesasge(request);

            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return JsonConvert.DeserializeObject<IList<CommunicationSetting>>((string)response.Response);
            }

            throw new Exception("Can not get communication settings");
        }

        public async Task<CommunicationSetting> Delete(CommunicationSetting setting, string source = null)
        {
            APIMessageRequest request = new APIMessageRequest()
            {
                RequestId = new Guid(),
                RequestName = "DELETE",
                Source = source == null ? "PORTAL" : source
            };

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "data", ParameterValue = setting.toJSON() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };

            var response = await sendMesasge(request);

            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return CommunicationSetting.fromJSON((string)response.Response);
            }

            throw new Exception("Can not delete communication setting");
        }

        public async Task<CommunicationSetting> Check(CommunicationSetting setting)
        {
            APIMessageRequest request = new APIMessageRequest()
            {
                RequestId = new Guid(),
                RequestName = "CHECK"
            };

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "data", ParameterValue = setting.toJSON() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };
            Console.WriteLine("sending 'CHECK' request to communication setting");
            var response = await sendMesasge(request);

            Console.WriteLine("GOT 'CHECK' response from communication setting");
            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return CommunicationSetting.fromJSON((string)response.Response);
            }

            throw new Exception("Can not get communication setting");
        }

        public async Task<IList<CommunicationSetting>> GetByType(CommunicationType type)
        {
            APIMessageRequest request = new APIMessageRequest()
            {
                RequestId = new Guid(),
                RequestName = "GET_BY_TYPE"
            };

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "type", ParameterValue = ((int)type).ToString() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };

            var response = await sendMesasge(request);

            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return JsonConvert.DeserializeObject<IList<CommunicationSetting>>((string)response.Response);
            }

            throw new Exception("Can not get communication settings");
        }

        public async Task<IList<CommunicationSetting>> GetByUser(int userId)
        {
            APIMessageRequest request = new APIMessageRequest()
            {
                RequestId = new Guid(),
                RequestName = "GET_BY_USER"
            };

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "userId", ParameterValue = userId.ToString() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };

            var response = await sendMesasge(request);

            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return JsonConvert.DeserializeObject<IList<CommunicationSetting>>((string)response.Response);
            }

            throw new Exception("Can not get communication settings");
        }

        public async Task<Dictionary<CommunicationType, bool>> GetUserFullSettings(int userId)
        {
            APIMessageRequest request = new APIMessageRequest()
            {
                RequestId = new Guid(),
                RequestName = "GET_USER_FULL_SETTINGS"
            };

            APIMessageRequestParameter param = new APIMessageRequestParameter() { ParameterName = "userId", ParameterValue = userId.ToString() };
            request.QueryParameters = new List<APIMessageRequestParameter>() { param };

            var response = await sendMesasge(request);

            if (response.Status == APIMessageResponse.RESPONSE_SUCCESS)
            {
                return JsonConvert.DeserializeObject<Dictionary<CommunicationType, bool>>((string)response.Response);
            }

            throw new Exception("Can not get communication settings");
        }
    }
}
