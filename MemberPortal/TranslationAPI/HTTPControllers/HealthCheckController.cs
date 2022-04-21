using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StagwellTech.SEIU.CommonCoreEntities.Services.Translation;

namespace TranslationAPI.HTTPControllers
{
    [Route("health-check")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ITranslationCacheService _cache;

        public HealthCheckController(ITranslationCacheService cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<Object>> Get()
        {
            var ping = _cache.Ping();
            var result = !String.IsNullOrEmpty(ping);

            return Ok(new
            {
                result = result,
                ping = ping
            });
        }
    }
}
