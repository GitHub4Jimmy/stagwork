using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using StagwellTech.ServiceBusRPC;
using StagwellTech.ServiceBusRPC.Entities;
using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using Microsoft.Extensions.DependencyInjection;
using StagwellTech.SEIU.CommonEntities.DTO.Dependent;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.SEIU.CommonCoreEntities.Handlers;

namespace StagwellTech.SEIU.API.DependentAPI
{
    public class DependentBusController : BaseBusController<DependentBusController, DependentService, IMPDependentPerson>
    {
        public static ILogger<DependentBusController> _logger;

        public DependentBusController(IServiceProvider serviceProvider, ILogger<DependentBusController> logger) : base(serviceProvider)
        {
            _logger = logger;
        }

        [ServiceBusRPCService(queueName: "dependent")]
        public static string ProcessRequest(string request)
        {
            var instance = GetInstance();
            var scope = instance._serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<DependentService>();
            var actions = new DependentBusControllerActions(service, _logger);

            Task<APIMessageResponse> task = Task.Run<APIMessageResponse>(async () => await instance._ProcessRequest(actions, request));
            APIMessageResponse result = task.GetAwaiter().GetResult();

            scope.Dispose();

            return result.toJSON();
        }
    }

    public class DependentBusControllerActions : BusControllerActions<DependentService>
    {
        public readonly ILogger<DependentBusController> _logger;

        public DependentBusControllerActions(DependentService service, ILogger<DependentBusController> logger) : base(service) 
        {
            _logger = logger;
        }

        [ServiceBusRPCMethod(requestName: "GET_DEPENDENTS")]
        public async Task<APIMessageResponse> GetDependents(APIMessageRequest apiRequest)
        {
            var personIdStr = apiRequest.GetParameterValue("personId");

            if (int.TryParse(personIdStr, out int personId))
            {
                try
                {
                    var objs = await _service.GetDependents(personId);
                    return APIMessageResponse.SendSuccessResponse(
                        apiRequest,
                        objs.GetType().FullName,
                        JsonConvert.SerializeObject(objs)
                    );
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    _logger.LogError(e.Message + " " + e.StackTrace);
                }
            }

            _logger.LogError($"{apiRequest.RequestName} not found");

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "GET_ENROLLMENT_FORMS")]
        public async Task<APIMessageResponse> GetDependentEnrollmentFormsByPersonId(APIMessageRequest apiRequest)
        {
            var personIdStr = apiRequest.GetParameterValue("personId");

            if (int.TryParse(personIdStr, out int personId))
                try
                {
                    List<PortalEnrollmentForm> portalEnrollmentForms = _service.GetEnrollmentFormsByPersonId(personId);

                    return APIMessageResponse.SendSuccessResponse(
                        apiRequest,
                        portalEnrollmentForms.GetType().FullName,
                        JsonConvert.SerializeObject(portalEnrollmentForms)
                    );

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    _logger.LogError(e.Message + " " + e.StackTrace);
                }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "GET_SUBMITTED_ENROLLMENT_FORMS")]
        public async Task<APIMessageResponse> GetSubmittedDependentEnrollmentFormsByPersonId(APIMessageRequest apiRequest)
        {
            var personIdStr = apiRequest.GetParameterValue("personId");

            if (int.TryParse(personIdStr, out int personId))
                try
                {
                    List<PortalEnrollmentForm> portalEnrollmentForms = _service.GetSubmittedEnrollmentFormsByPersonId(personId);

                    return APIMessageResponse.SendSuccessResponse(
                        apiRequest,
                        portalEnrollmentForms.GetType().FullName,
                        JsonConvert.SerializeObject(portalEnrollmentForms)
                    );

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    _logger.LogError(e.Message + " " + e.StackTrace);
                }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "GET_ENROLLMENT_FORM")]
        public async Task<APIMessageResponse> GetDependentEnrollmentForm(APIMessageRequest apiRequest)
        {
            var idStr = apiRequest.GetParameterValue("id");

            if (Guid.TryParse(idStr, out Guid id))
                try
                {
                    PortalEnrollmentForm portalEnrollmentForm = await _service.GetEnrollmentForm(id);

                    return APIMessageResponse.SendSuccessResponse(
                        apiRequest,
                        portalEnrollmentForm.GetType().FullName,
                        JsonConvert.SerializeObject(portalEnrollmentForm)
                    );

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    _logger.LogError(e.Message + " " + e.StackTrace);
                }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "ADD_ENROLLMENT_FORM")]
        public async Task<APIMessageResponse> AddDependentEnrollmentForm(APIMessageRequest apiRequest)
        {
            var dataStr = apiRequest.GetParameterValue("data");
            
            try
            {
                PortalEnrollmentForm item = PortalEnrollmentForm.fromJSON(dataStr);
                PortalEnrollmentForm portalEnrollmentForm = await _service.AddEnrollmentForm(item);

                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    portalEnrollmentForm.GetType().FullName,
                    portalEnrollmentForm.toJSON()
                );

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "UPDATE_ENROLLMENT_FORM")]
        public async Task<APIMessageResponse> UpdateDependentEnrollmentForm(APIMessageRequest apiRequest)
        {
            var dataStr = apiRequest.GetParameterValue("data");

            try
            {
                PortalEnrollmentForm item = PortalEnrollmentForm.fromJSON(dataStr);
                PortalEnrollmentForm portalEnrollmentForm = await _service.UpdateEnrollmentForm(item);

                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    portalEnrollmentForm.GetType().FullName,
                    portalEnrollmentForm.toJSON()
                );

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "DELETE_ENROLLMENT_FORM")]
        public async Task<APIMessageResponse> DeleteDependentEnrollmentForm(APIMessageRequest apiRequest)
        {
            var dataStr = apiRequest.GetParameterValue("data");

            try
            {
                var id = Guid.Parse(dataStr);
                PortalEnrollmentForm portalEnrollmentForm = await _service.DeleteEnrollmentForm(id);

                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    portalEnrollmentForm.GetType().FullName,
                    portalEnrollmentForm.toJSON()
                );

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "GET_FRIENDLY_ID")]
        public async Task<APIMessageResponse> GetFriendlyId(APIMessageRequest apiRequest)
        {
            try
            {
                var result = _service.GetFriendlyId();
                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    result.GetType().FullName,
                    JsonConvert.SerializeObject(result)
                );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "SUBMIT_ENROLLMENT_FORM")]
        public async Task<APIMessageResponse> SubmitForm(APIMessageRequest apiRequest)
        {
            var dataStr = apiRequest.GetParameterValue("data");
            try
            {
                PortalEnrollmentForm item = PortalEnrollmentForm.fromJSON(dataStr);
                var result = _service.SubmitForm(item);
                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    result.GetType().FullName,
                    JsonConvert.SerializeObject(result)
                );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "UPDATE_CRM_FORM")]
        public async Task<APIMessageResponse> UpdateCrmForm(APIMessageRequest apiRequest)
        {
            var dataStr = apiRequest.GetParameterValue("data");
            try
            {
                DependentEnrollmentCrmForm item = DependentEnrollmentCrmForm.fromJSON(dataStr);
                var result = _service.UpdateCrmForm(item);
                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    result.GetType().FullName,
                    JsonConvert.SerializeObject(result)
                );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return APIMessageResponse.Send404Response(apiRequest, $"{apiRequest.RequestName} not found");
        }

        [ServiceBusRPCMethod(requestName: "GET_ENROLLMENT_DOCUMENTS")]
        public async Task<APIMessageResponse> GetEnrollmentDocuments(APIMessageRequest apiRequest)
        {
            var dataStr = apiRequest.GetParameterValue("data");
            try
            {
                Guid item = Guid.Parse(JsonConvert.DeserializeObject<string>(dataStr));
                var result = await _service.GetEnrollmentDocuments(item);
                return APIMessageResponse.SendSuccessResponse(
                    apiRequest,
                    result.GetType().FullName,
                    JsonConvert.SerializeObject(result)
                );
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
