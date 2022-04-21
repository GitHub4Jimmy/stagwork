using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StagwellTech.SEIU.CommonCoreEntities.AuthSEIU;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{

    public class SEIUAuthHTTPController : ControllerBase
    {
        protected readonly ISEIUAuthenticationService authService;
        public SEIUAuthHTTPController(ISEIUAuthenticationService authService)
        {
            this.authService = authService;

        }
        protected async Task<MPPerson> CurrentPerson()
        {
            return await authService.GetCurrentPerson(User);
        }
        protected async Task<UserSettings> CurrentUserSettings()
        {
            return await authService.GetCurrentUserSettings(User);
        }
        
        protected string GetCurrentUserEmail()
        {
            return authService.GetCurrentUserEmail(User);
        }
        protected bool IsSignatureUsed()
        {
            return authService.IsSignatureUsed(User);
        }
        
        protected string GetCurrentDocumentIdOrName()
        {
            return authService.GetCurrentDocumentIdOrName(User);
        }

        protected async Task<int> CurrentPersonId()
        {
            var settings = await CurrentUserSettings();
            int pid;
            if (int.TryParse(settings.PersonId, out pid))
            {
                return pid;
            }
            else
            {
                return -1;
            }
        }
    }

    public class BaseHTTPController<ServiceType, BusClientType> : SEIUAuthHTTPController where ServiceType : IGetCount where BusClientType : BaseClientSingleton
    {
        protected readonly ServiceType _service;
        protected readonly BusClientType _busClient;

        public BaseHTTPController(ServiceType context, ISEIUAuthenticationService authService, BusClientType busClient) : base(authService)
        {
            _service = context;
            _busClient = busClient;
        }


        [AllowAnonymous]
        [HttpGet("health")]
        public ActionResult Health()
        {
            return Ok("OK");
        }

        [AllowAnonymous]
        [HttpGet("health-db")]
        public async Task<ActionResult> HealthDB()
        {
            try
            {
                await _busClient.HealthCheck();
                await _service.GetFirstOrDefault();
                return Ok("OK");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        protected static string Error()
        {
            return "An error has occured";
        }
    }
}
