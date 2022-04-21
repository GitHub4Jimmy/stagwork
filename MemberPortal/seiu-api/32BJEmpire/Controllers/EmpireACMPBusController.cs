using SEIU32BJEmpire.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using StagwellTech.ServiceBusRPC;
using StagwellTech.ServiceBusRPC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.DataModels.Empire;

namespace SEIU32BJEmpire.Controllers
{
    public class EmpireACMPBusController : BaseBusController<EmpireACMPBusController, EmpireACMPService, IEmpireACMPEncryptedRequest>
    {

        public EmpireACMPBusController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [ServiceBusRPCService(queueName: "32BJEmpireACMPQueue")]
        public static string ProcessRequest(string request)
        {
            var instance = GetInstance();
            var scope = instance._serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<EmpireACMPService>();
            var actions = new EmpireACMPBusControllerActions(service);

            Task<APIMessageResponse> task = Task.Run<APIMessageResponse>(async () => await instance._ProcessRequest(actions, request));
            APIMessageResponse result = task.GetAwaiter().GetResult();

            scope.Dispose();

            return result.toJSON();
        }

    }

    public class EmpireACMPBusControllerActions : BusControllerActions<EmpireACMPService>
    {

        public EmpireACMPBusControllerActions(EmpireACMPService service) : base(service) { }

        [ServiceBusRPCMethod(requestName: "RESPONSE")]
        public async Task<APIMessageResponse> Response(APIMessageRequest apiRequest)
        {
            var responseStr = apiRequest.GetParameterValue("data");

            var response = JsonConvert.DeserializeObject<SEIUEmpireResponseJson>(responseStr);
            
            // TODO: need to accept full response message, save it and pass it on to Empire
            var item = _service.AcceptResponse(new SEIUEmpireResponse(response));

            if (item != null)
            {
                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    item.GetType().FullName,
                    JsonConvert.SerializeObject(item)
                );
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }
    }
 }
