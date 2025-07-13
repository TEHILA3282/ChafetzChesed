using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using ChafetzChesed.Common.Models;
using ChafetzChesed.Common.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly IExternalFormService _externalFormService;
        private readonly IExternalUserSyncService _externalUserSyncService;

        public AdminController(
            IRegistrationService registrationService,
            IExternalFormService externalFormService,
            IExternalUserSyncService externalUserSyncService)
        {
            _registrationService = registrationService;
            _externalFormService = externalFormService;
            _externalUserSyncService = externalUserSyncService;
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Registration>>> GetPendingAsync()
        {
            var institutionIdClaim = User.FindFirst("InstitutionId")?.Value;
            if (!int.TryParse(institutionIdClaim, out var institutionId))
                return Unauthorized("InstitutionId missing in token.");

            var pending = await _registrationService.GetPendingAsync(institutionId);
            return Ok(pending);
        }


        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody] UpdateStatusDto request)
        {
            var result = await _registrationService.UpdateStatusAsync(request.RegistrationId.ToString(), request.NewStatus);
            if (!result)
                return NotFound("משתמש לא נמצא");

            return Ok("הסטטוס עודכן בהצלחה");
        }

        [HttpGet("external-forms")]
        public async Task<ActionResult<List<ExternalForm>>> GetExternalFormsAsync()
        {
            var forms = await _externalFormService.GetFormsAsync();
            return Ok(forms);
        }

        [HttpPost("sync-users")]
        public async Task<IActionResult> SyncUsersAsync()
        {
            var updatedCount = await _externalUserSyncService.SyncAsync();
            return Ok($"{updatedCount} משתמשים עודכנו");
        }
    }
}
