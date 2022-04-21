using System;
using System.Collections.Generic;
using SEIU32BJEmpire.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.CommonEntities.Utils.Encryption;
using StagwellTech.SEIU.CommonEntities.DataModels.Empire;
using System.Threading.Tasks;
using SEIU32BJEmpire.Data;
using SEIU32BJEmpire.Helpers;

namespace SEIU32BJEmpire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpireACMPController : Controller
    {
        private readonly ILogger<EmpireACMPController> _logger;
        private readonly EmpireACMPService service;

        public EmpireACMPController(ILogger<EmpireACMPController> logger, EmpireACMPService service)
        {
            _logger = logger;
            this.service = service;
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "pong";
        }

        [HttpGet("validate-token")]
        public bool ValidateToken()
        {
            Request.Headers.TryGetValue("Authorization", out var headerValue);
            return TokenValidator.Validate(headerValue, out EmpirePostErrorResponse errorResponse);
        }

        [HttpGet("test-anthem-auth")]
        public Object TestAnthemAuth()
        {
            try
            {
                var result = service.GetEmpireAuthToken();
                if(result == null)
                {
                    return new
                    {
                        message = "Request failed - was null"
                    };

                }
                return new {
                    statusCode = result.StatusCode,
                    content = result.Content
                };
            }
            catch (Exception e)
            {
                return new
                {
                    error = "Exception",
                    message = e.Message,
                    stackTrace = e.StackTrace
                };
            }
        }

        [HttpGet("resend-response-to-anthem")]
        public Object ResendToAnthem(Guid id)
        {
            try
            {
                var result = service.ResendResponse(id);
                return result;
            } catch (Exception e)
            {
                return new
                {
                    error = "Exception",
                    message = e.Message,
                    stackTrace = e.StackTrace
                };
            }
        }

        [HttpPost("encrypted")]
        public async Task<IEmpirePostResponse> PostEncrypted([FromBody] EncryptedData data)
        {
            Request.Headers.TryGetValue("Authorization", out var headerValue);

            if(!TokenValidator.Validate(headerValue, out EmpirePostErrorResponse errorResponse))
            {
                return errorResponse;
            }

            List<EmpireException> exceptions = new List<EmpireException>();
            try
            {
                var result = await service.PostEncrypted(data);
                if (result != null && result.authorizationDetails != null)
                {
                    return new EmpirePostSuccessResponse(){ ackid = result.authorizationDetails.mmsSystemId };
                } else
                {
                    exceptions.Add(new EmpireException()
                    {
                        code = "E",
                        message = "Empty",
                        detail = "AuthorizationDetails are missing"
                    });
                }
            } catch(Exception e)
            {
                exceptions.Add(new EmpireException()
                {
                    code = "E",
                    message = e.Message,
                    detail = "Unaible to parse or save Data"
                });
            }

            return new EmpirePostErrorResponse()
            {
                ackid = "",
                type = "400",
                exceptionList = exceptions
            };

        }

    }
}