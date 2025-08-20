using ChafetzChesed.BLL.Services;
using ChafetzChesed.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChafetzChesed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstitutionsController : ControllerBase
    {
        private readonly IInstitutionResolver _resolver;

        public InstitutionsController(IInstitutionResolver resolver)
        {
            _resolver = resolver;
        }

        [HttpGet("public-info")]
        [AllowAnonymous]
        public async Task<ActionResult<InstitutionPublicInfoDto>> GetPublicInfo()
        {
            var host = HttpContext.Request.Host.Host;
            var sub = host.Split('.').FirstOrDefault() ?? "localhost";
            var inst = await _resolver.GetBySubdomainAsync(sub);
            if (inst == null) return NotFound();

            return new InstitutionPublicInfoDto
            {
                InstitutionId = inst.ID,
                Phone = inst.ContactPhone ?? "03-0000000",
                AvailabilityText = inst.AvailabilityText ?? "א׳–ו׳ 09:30–10:30"
            };
        }
    }

}
