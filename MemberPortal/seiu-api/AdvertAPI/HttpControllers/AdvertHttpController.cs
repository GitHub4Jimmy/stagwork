using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using StagwellTech.SEIU.CommonEntities.DTO.Advert;
using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.CommonCoreEntities.AuthSEIU;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Advert;
using System.Collections.Generic;
using StagwellTech.SEIU.CommonEntities.Portal.Advert;
using StagwellTech.SEIU.CommonEntities.BusClients;

namespace StagwellTech.SEIU.API.AdvertAPI
{
    [Route("api/advert")]
    [EnableCors("AllowAll")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Constants.SEIUScheme)]
    public class AdvertHTTPController : BaseHTTPController<AdvertService, AdvertClient>
    {
        public static ILogger<AdvertHTTPController> _logger;

        public AdvertHTTPController(AdvertService context, ISEIUAuthenticationService authService, ILogger<AdvertHTTPController> logger) : base(context, authService, AdvertClient.Instance)
        {
            _logger = logger;
        }

        [HttpGet("advert-types")]
        public async Task<ActionResult<List<AdvertType>>> GetAdvertTypes()
        {
            var res = await _service.GetAdvertTypes();
            return Ok(res);
        }

        [HttpGet("find-adverts")]
        public async Task<ActionResult<List<AdvertDisplay>>> FindAdverts([FromQuery] List<int> adTypeId, [FromQuery] int limit = 5)
        {
            var cpid = await CurrentPersonId();
            var res = await _service.FindAdverts(cpid, adTypeId, limit);
            return Ok(res);
        }
    }
}
