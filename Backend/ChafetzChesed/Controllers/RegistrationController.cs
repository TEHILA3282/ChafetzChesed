using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ChafetzChesed.Common.DTOs;
using ChafetzChesed.BLL.Services;
using System.Security.Claims;
using ChafetzChesed.DAL.Data;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;




namespace ChafetzChesed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly AppDbContext _context;

        public RegistrationController(
    IRegistrationService registrationService,
    AppDbContext context)
        {
            _registrationService = registrationService;
            _context = context;
        }

        // GET: api/Registration
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetAll()
        {
            var registrations = await _registrationService.GetAllAsync();
            return Ok(registrations);
        }

        // GET: api/Registration/123456789
        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetById(string id)
        {
            var registration = await _registrationService.GetByIdAsync(id);
            if (registration == null)
                return NotFound("Registration not found.");
            return Ok(registration);
        }

        // POST: api/Registration
        [HttpPost]
        public async Task<ActionResult<Registration>> Add([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest("Validation failed: " + string.Join("; ", errors));
            }
            var existing = await _registrationService.GetAllAsync();
            if (existing.Any(r => r.Email == registration.Email))
                return BadRequest("Email already exists");
            if (existing.Any(r => r.ID == registration.ID))
                return BadRequest("ID already exists");

            try
            {
                registration.Password = HashPassword(registration.Password);

                if (registration.Role == "Admin")
                    registration.RegistrationStatus = "מאושר";
                else
                    registration.RegistrationStatus = "ממתין"; 

                registration.StatusUpdatedAt = DateTime.Now;

                var added = await _registrationService.AddAsync(registration);
                return CreatedAtAction(nameof(GetById), new { id = added.ID }, added);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception: " + ex.Message);
            }
        }

        // PUT: api/Registration
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Registration registration)
        {
            var success = await _registrationService.UpdateAsync(registration);
            if (!success)
                return NotFound("Registration not found.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _registrationService.DeleteAsync(id);
            if (!success)
                return NotFound("Registration not found.");
            return NoContent();
        }
        [HttpGet("check-exists")]
        public async Task<ActionResult<bool>> CheckEmailOrIdExists([FromQuery] string email, [FromQuery] string id, [FromQuery] int institutionId)
        {
            var exists = await _registrationService.ExistsAsync(email, id, institutionId);
            return Ok(exists);
        }
        [HttpPut("update-personal")]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalDetails([FromBody] RegistrationUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("❌ לא נמצאה ת\"ז ב-Claim");
                return Unauthorized("Missing user ID from NameIdentifier");
            }

            Console.WriteLine($"✅ ת\"ז שנשלפה מה-Claim: {userId}");

            var result = await _registrationService.UpdatePartialAsync(userId, dto);

            if (!result)
                return BadRequest("נכשל לעדכן את הפרטים האישיים");

            return Ok("הפרטים האישיים עודכנו בהצלחה ✅");
        }
        [HttpPut("update-bank")]
        [Authorize]
        public async Task<IActionResult> UpdateBankDetails([FromBody] BankAccountUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("❌ לא נמצאה ת\"ז ב-Claim");
                return Unauthorized("Missing user ID from NameIdentifier");
            }
            Console.WriteLine($"✅ ת\"ז שנשלפה מה-Claim: {userId}");


            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Missing user ID");

            var existingAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.RegistrationId == userId);

            if (existingAccount != null)
            {
                existingAccount.BankNumber = dto.BankNumber;
                existingAccount.BranchNumber = dto.BranchNumber;
                existingAccount.AccountNumber = dto.AccountNumber;
                existingAccount.AccountOwnerName = dto.AccountOwnerName;
                existingAccount.HasDirectDebit = dto.HasDirectDebit;
            }
            else
            {
                var newAccount = new BankAccount
                {
                    RegistrationId = userId,
                    BankNumber = dto.BankNumber,
                    BranchNumber = dto.BranchNumber,
                    AccountNumber = dto.AccountNumber,
                    AccountOwnerName = dto.AccountOwnerName,
                    HasDirectDebit = dto.HasDirectDebit 
                };
                _context.BankAccounts.Add(newAccount);
            }

            await _context.SaveChangesAsync();
            return Ok("✅ פרטי החשבון עודכנו בהצלחה");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
