using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StagwellTech.SEIU.CommonCoreEntities.AuthSEIU;
using StagwellTech.SEIU.CommonCoreEntities.Services.Translation;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation.Models;

namespace TranslationAPI.HTTPControllers
{
    [Route("api/translate")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        private readonly ITranslationService _service;

        public TranslateController(ITranslationService service)
        {
            _service = service;
        }

        // POST api/values
        [HttpPost]
        [Authorize(AuthenticationSchemes = Constants.SEIUScheme)]
        public async Task<List<string>> Post([FromBody] TranslationRequest request)
        {
            return await _service.Translate(request);
        }

    }
}
