using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.BLL.Services;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using ChafetzChesed.DAL.Data;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        public AuthController(IRegistrationService registrationService, JwtService jwtService, AppDbContext context)
        {
            _registrationService = registrationService;
            _jwtService = jwtService;
            _context = context;
        }

        private Registration? GetLoggedInUser() => HttpContext.Items["User"] as Registration;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var users = await _registrationService.GetAllAsync();
            string hashedPassword = HashPassword(request.Password);

            var idRaw = request.Identifier.Trim();
            var idNoLeadingZeros = idRaw.TrimStart('0');
            var idWithPadding = idNoLeadingZeros.PadLeft(9, '0');

            var user = users.FirstOrDefault(u =>
                (u.Email.ToLower() == idRaw.ToLower()
                 || u.ID == idRaw
                 || u.ID == idNoLeadingZeros
                 || u.ID == idWithPadding) &&
                u.Password == hashedPassword &&
                u.InstitutionId == request.InstitutionId
            );

            if (user == null)
            {
                return Unauthorized(new { message = "אימייל, תז, סיסמה או מוסד אינם תואמים" });
            }

            var role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;
            var token = _jwtService.GenerateToken(user.ID.ToString(), user.Email, user.InstitutionId, role);

            return Ok(new
            {
                message = "התחברת בהצלחה",
                token,
                user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.ID,
                    user.Email,
                    user.InstitutionId,
                    user.Role,
                    user.RegistrationStatus
                }
            });
        }



        [HttpGet("get-bank-details")]
        [Authorize]
        public async Task<IActionResult> GetBankDetails()
        {
            var user = GetLoggedInUser();
            if (user == null)
                return Unauthorized("משתמש לא מאומת");

            Console.WriteLine($"🔍 משתמש מחובר: {user.ID} ({user.Email})");

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(b => b.RegistrationId == user.ID);

            if (bankAccount == null)
                return NotFound("Bank account not found");

            var dto = new BankAccountUpdateDto
            {
                BankNumber = bankAccount.BankNumber,
                BranchNumber = bankAccount.BranchNumber,
                AccountNumber = bankAccount.AccountNumber,
                AccountOwnerName = bankAccount.AccountOwnerName,
                HasDirectDebit = bankAccount.HasDirectDebit
            };

            return Ok(dto);
        }

        [HttpGet("get-current-user")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var user = GetLoggedInUser();
            if (user == null)
                return Unauthorized("משתמש לא מאומת");

            return Ok(user);
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _registrationService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "משתמש לא נמצא" });

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.ID,
                user.Email,
                user.InstitutionId,
                user.Role,
                user.RegistrationStatus
            });
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
