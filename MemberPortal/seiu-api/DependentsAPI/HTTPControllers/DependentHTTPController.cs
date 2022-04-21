using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StagwellTech.SEIU.CommonCoreEntities.BaseAPI;
using Microsoft.AspNetCore.Cors;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Dependent;
using Microsoft.AspNetCore.Authorization;
using StagwellTech.SEIU.CommonCoreEntities.AuthSEIU;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.DataModels.DTO.Dependent;

namespace StagwellTech.SEIU.API.DependentAPI
{
    [Route("api/dependents")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class DependentHTTPController : BaseHTTPController<DependentService, DependentClient>
    {
        public DependentHTTPController(DependentService context, ISEIUAuthenticationService authService) : base(context, authService, DependentClient.Instance) { }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Constants.SEIUScheme)]
        public async Task<ActionResult<List<MPDependentPerson>>> Get()
        {
            var cpid = await CurrentPersonId();
            var Event = await _service.GetDependents(cpid);
            return Ok(Event);
        }

        [HttpGet("applicants")]
        [Authorize(AuthenticationSchemes = Constants.SEIUScheme)]
        public async Task<ActionResult<List<DependentApplicant>>> GetApplicants()
        {
            var applicants = new List<DependentApplicant>();
            var personId = await CurrentPersonId();
            var dependents = await _service.GetDependents(personId);
            dependents.ForEach(x =>
            {
                applicants.Add(new DependentApplicant().FromMPDependentPerson(x));
            });
            return Ok(applicants);
        }

    }
}
