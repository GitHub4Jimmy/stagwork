using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using StagwellTech.SEIU.CommonCoreEntities.Services.Translation;
using StagwellTech.SEIU.CommonCoreEntities.Services.Translation.Models;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation.Models;
using StagwellTech.ServiceBusRPC;
using StagwellTech.ServiceBusRPC.Entities;

namespace TranslationAPI.BusControllers
{

    public class TranslationCacheBusController : BaseBusController<TranslationCacheBusController, ITranslationService, List<string>>
    {
        public static ILogger<TranslationCacheBusController> _logger;

        public TranslationCacheBusController(IServiceProvider serviceProvider, ILogger<TranslationCacheBusController> logger) : base(serviceProvider)
        {
            _logger = logger;
        }

        [ServiceBusRPCService(queueName: "translation-cache")]
        public static string ProcessRequest(string request)
        {
            var instance = GetInstance();
            var scope = instance._serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<ITranslationService>();
            var actions = new TranslationCacheBusControllerActions(service, _logger);

            Task<APIMessageResponse> task = Task.Run<APIMessageResponse>(async () => await instance._ProcessRequest(actions, request));
            APIMessageResponse result = task.GetAwaiter().GetResult();

            scope.Dispose();

            return result.toJSON();
        }
    }

    public class TranslationCacheBusControllerActions : BusControllerActions<ITranslationService>
    {
        public readonly ILogger<TranslationCacheBusController> _logger;

        public TranslationCacheBusControllerActions(ITranslationService service, ILogger<TranslationCacheBusController> logger) : base(service)
        {
            _logger = logger;
        }

        //GET_UPCOMING
        [ServiceBusRPCMethod(requestName: "TRANSLATE")]
        public async Task<APIMessageResponse> Translate(APIMessageRequest apiRequest)
        {
            var requestStr = apiRequest.GetParameterValue("data");

            try
            {
                var request = TranslationRequest.fromJSON(requestStr);

                var res =  await _service.Translate(request);

                if (res != null)
                {
                    return APIMessageResponse.SendSuccessResponse(
                        apiRequest,
                        res.GetType().FullName,
                        JsonConvert.SerializeObject(res)
                    );
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }
    }
}
