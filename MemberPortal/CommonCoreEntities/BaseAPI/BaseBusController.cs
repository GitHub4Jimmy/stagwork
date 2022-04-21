using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.DataModels.Portal;
using StagwellTech.ServiceBusRPC.Entities;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public abstract class BusControllerActions<TService> : IBusControllerActions where TService : IGetCount
    {
        public readonly ILogger logger;

        protected TService _service;
        public BusControllerActions(TService service)
        {
            _service = service;
        }

        public BusControllerActions(ILogger<BusControllerActions<TService>> Logger)
        {
            logger = Logger;
        }

        [ServiceBusRPCMethod(requestName: "HEALTH_CHECK")]
        public async Task<APIMessageResponse> HealthCheck(APIMessageRequest apiRequest)
        {
            try
            {
                var count = await _service.GetFirstOrDefault();
                return APIMessageResponse.SendSuccessResponse(
                            apiRequest,
                            count.GetType().FullName,
                            count.ToString()
                        );
            } catch(Exception e)
            {
                return APIMessageResponse.Send500Response(apiRequest, e.Message);
            }
        }

    }

    public abstract class BaseBusController<TSelf, TBaseService, TEntity> : IBusController
        where TSelf : BaseBusController<TSelf, TBaseService, TEntity>
    {

        protected static TSelf _instance = default(TSelf);
        protected IServiceProvider _serviceProvider;

        public static TSelf GetInstance()
        {
            return _instance;
        }

        protected BaseBusController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _instance = (TSelf)this;
        }

        public Task<APIMessageResponse> _ProcessRequest(IBusControllerActions controllerActions, string request)
        {
            APIMessageRequest apiRequest = APIMessageRequest.fromJSON(request);

            var requestName = apiRequest.RequestName.ToUpper();

            try
            {
                string enabledVar = Environment.GetEnvironmentVariable("ENABLE_SERVICE_BUS_LOGS");
                bool enabled = false;
                if (enabledVar != null && bool.TryParse(enabledVar, out enabled))
                {
                    if (enabled)
                    {
                        var scope = _serviceProvider.CreateScope();
                        SeiuContext context = scope.ServiceProvider.GetService<SeiuContext>();
                        context.APIMessageRequestLogs.Add(new APIMessageRequestLog(controllerActions.GetType().Name, apiRequest));
                        context.SaveChanges();
                        scope.Dispose();
                    }
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            Console.WriteLine("requestName=" + requestName);

            foreach (var method in controllerActions.GetType().GetMethods())
            {

                ServiceBusRPCMethodAttribute serviceBusMethodAttr = (ServiceBusRPCMethodAttribute)method.GetCustomAttribute(typeof(ServiceBusRPCMethodAttribute));

                if (serviceBusMethodAttr != null && serviceBusMethodAttr.requestName == requestName)
                {
                    return (Task<APIMessageResponse>)method.Invoke(controllerActions, new[] { apiRequest });
                }

            }

            var response404 = APIMessageResponse.Send404Response(apiRequest);

            return Task.FromResult(response404);
        }

    }
}