using StagwellTech.SEIU.CommonEntities.CRM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.CRM
{
    public interface ICRMMessageHandler
    {
        Task<CRMMessageResponse> handleMessage(CRMMessageRequest message);

        Task<CRMMessageResponse> portalDelete(CRMMessageRequest message);
        Task<CRMMessageResponse> portalUpdate(CRMMessageRequest message);
        Task<CRMMessageResponse> portalCreate(CRMMessageRequest message);
        Task<CRMMessageResponse> portalCreateOrUpdate(CRMMessageRequest message);

        Task<CRMMessageResponse> crmDelete(CRMMessageRequest message);
        Task<CRMMessageResponse> crmUpdate(CRMMessageRequest message);
        Task<CRMMessageResponse> crmCreate(CRMMessageRequest message);
        Task<CRMMessageResponse> crmCreateOrUpdate(CRMMessageRequest message);
    }
}
