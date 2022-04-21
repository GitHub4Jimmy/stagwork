using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using StagwellTech.ServiceBusRPC;
using StagwellTech.SEIU.CommonEntities;
using StagwellTech.ServiceBusRPC.Entities;
using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using StagwellTech.SEIU.CommonEntities.DTO.Advert;
using Newtonsoft.Json;
using StagwellTech.SEIU.CommonEntities.Utils;
using System.Collections.Generic;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Advert;

namespace StagwellTech.SEIU.API.AdvertAPI
{
    public class AdvertBusController : BaseBusController<AdvertBusController, AdvertService, IAdvertDisplay>
    {

        public AdvertBusController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [ServiceBusRPCService(queueName: "advert")]
        public static string ProcessRequest(string request)
        {
            var instance = GetInstance();
            var scope = instance._serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<AdvertService>();
            var actions = new AdvertBusControllerActions(service);

            Task<APIMessageResponse> task = Task.Run<APIMessageResponse>(async () => await instance._ProcessRequest(actions, request));
            APIMessageResponse result = task.GetAwaiter().GetResult();

            scope.Dispose();

            return result.toJSON();
        }

    }



    public class AdvertBusControllerActions : BusControllerActions<AdvertService>
    {

        public AdvertBusControllerActions(AdvertService service) : base(service) { }

        [ServiceBusRPCMethod(requestName: "FIND_ADVERTS")]
        public async Task<APIMessageResponse> FindAdverts(APIMessageRequest apiRequest)
        {
            var personId = apiRequest.GetParameterValue("personId").ToNullableInt();
            var limit = apiRequest.GetParameterValue("limit").ToNullableInt();
            var adTypeIds = JsonConvert.DeserializeObject<List<int>>(apiRequest.GetParameterValue("adTypeIds"));

            try
            {
                if (personId.HasValue && adTypeIds.HasValue() && limit.HasValue)
                {
                    var items = await _service.FindAdverts(personId.Value, adTypeIds, limit.Value);

                    return APIMessageResponse.SendSuccessResponse(
                        apiRequest,
                        items.GetType().FullName,
                        JsonConvert.SerializeObject(items)
                    );
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "GET_ADVERT_TYPES")]
        public async Task<APIMessageResponse> GetAdvertTypes(APIMessageRequest apiRequest)
        {
            try
            {
                var item = await _service.GetAdvertTypes();

                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    item.GetType().FullName,
                    JsonConvert.SerializeObject(item)
                );

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

    }
}
