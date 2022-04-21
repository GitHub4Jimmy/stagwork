using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TranslationAPI.HTTPControllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Object>> Get()
        {
            return Ok(new
            {
                result = "test controller - v3" 
            });
        }
    }
}
