using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using StagwellTech.ServiceBusRPC;
using StagwellTech.ServiceBusRPC.Entities;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public interface IBusControllerActions
    {
        Task<APIMessageResponse> HealthCheck(APIMessageRequest apiRequest);
    }
    public interface IBusController
    {
        Task<APIMessageResponse> _ProcessRequest(IBusControllerActions controllerActions, string request);
    }
}
