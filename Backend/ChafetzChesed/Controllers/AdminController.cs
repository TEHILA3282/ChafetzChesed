using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using ChafetzChesed.Common.Models;
using ChafetzChesed.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using ChafetzChesed.DAL.Data;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly IExternalFormService _externalFormService;
        private readonly IExternalUserSyncService _externalUserSyncService;
        private readonly AppDbContext _context;

        public AdminController(
            IRegistrationService registrationService,
            IExternalFormService externalFormService,
            IExternalUserSyncService externalUserSyncService,
             AppDbContext context)
        {
            _registrationService = registrationService;
            _externalFormService = externalFormService;
            _externalUserSyncService = externalUserSyncService;
            _context = context;
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

        [HttpGet("external-forms")]
        public async Task<ActionResult<List<ExternalForm>>> GetExternalFormsAsync()
        {
            var forms = await _externalFormService.GetFormsAsync();
            return Ok(forms);
        }

        [HttpPost("sync-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SyncUsersAsync()
        {
            try
            {
                var updatedCount = await _externalUserSyncService.SyncAsync();
                return Ok(new { message = " משתמשים עודכנו בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "אירעה שגיאה בסנכרון המשתמשים" });
            }
        }

        [HttpGet("approved")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Registration>>> GetApprovedAsync()
        {
            var institutionIdClaim = User.FindFirst("InstitutionId")?.Value;
            if (!int.TryParse(institutionIdClaim, out var institutionId))
                return Unauthorized("InstitutionId missing in token.");

            var approved = await _registrationService.GetByStatusAsync(institutionId, "מאושר");
            return Ok(approved);
        }

        [HttpGet("rejected")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Registration>>> GetRejectedAsync()
        {
            var institutionIdClaim = User.FindFirst("InstitutionId")?.Value;
            if (!int.TryParse(institutionIdClaim, out var institutionId))
                return Unauthorized("InstitutionId missing in token.");

            var rejected = await _registrationService.GetByStatusAsync(institutionId, "נדחה");
            return Ok(rejected);
        }
        [HttpPost("update-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody] UpdateStatusDto request)
        {
            var result = await _registrationService.UpdateStatusAsync(request.RegistrationId, request.NewStatus);
            if (!result)
                return NotFound("משתמש לא נמצא");

            return Ok(new { message = "הסטטוס עודכן בהצלחה" });
        }
        [HttpGet("test-local")]
        public IActionResult TestLocal()
        {
            var filePath = @"C:\Users\משתמש\Downloads\account_actions_example.xlsx";
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

            var actions = new List<AccountAction>();

            foreach (var row in rows)
            {
                var action = new AccountAction
                {
                    InstitutionId = 1,
                    Zeout = long.Parse(row.Cell(2).GetString()),
                    Seder = int.Parse(row.Cell(3).GetString()),
                    Perut = row.Cell(4).GetString(),
                    Important = int.Parse(row.Cell(5).GetString()),
                    CreatedAt = DateTime.Now
                };
                actions.Add(action);
            }

            _context.AccountActions.AddRange(actions);
            _context.SaveChanges();

            return Ok($"✅ Loaded {actions.Count} rows from local file.");
        }


    }
}
