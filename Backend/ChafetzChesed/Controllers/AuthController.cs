using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.BLL.Services;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChafetzChesed.DAL.Data;
using Microsoft.EntityFrameworkCore;


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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var users = await _registrationService.GetAllAsync();

            string hashedPassword = HashPassword(request.Password);

            Console.WriteLine("============ LOGIN DEBUG START ============");
            Console.WriteLine($"REQUEST IDENTIFIER: {request.Identifier}");
            Console.WriteLine($"REQUEST PASSWORD (HASHED): {hashedPassword}");
            Console.WriteLine($"REQUEST INSTITUTION: {request.InstitutionId}");

            var user = users.FirstOrDefault(u =>
                (u.Email.ToLower() == request.Identifier.ToLower() || u.ID == request.Identifier) &&
                u.Password == hashedPassword &&
                u.InstitutionId == request.InstitutionId
            );

            if (user == null)
            {
                Console.WriteLine("❌ לא נמצא משתמש תואם");
                return Unauthorized(new { message = "אימייל, תז, סיסמה או מוסד אינם תואמים" });
            }

            Console.WriteLine($"✅ נמצא משתמש: {user.Email} ({user.Role})");

            var role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;
            var token = _jwtService.GenerateToken(user.ID.ToString(), user.Email, user.InstitutionId, role);

            Console.WriteLine("============ LOGIN DEBUG END ============\n");

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
                    user.Role
                }
            });
        }

        [HttpGet("get-bank-details")]
        [Authorize]
        public async Task<IActionResult> GetBankDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("❌ לא נמצאה ת\"ז ב-Claim");
                return Unauthorized("Missing user ID from NameIdentifier");
            }

            Console.WriteLine($"🔍 ת\"ז שנשלפה מה-Claim: {userId}");

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(b => b.RegistrationId == userId);

            if (bankAccount == null)
            {
                Console.WriteLine("❌ לא נמצאו פרטי חשבון בנק");
                return NotFound("Bank account not found");
            }

            Console.WriteLine("✅ פרטי חשבון נטענו בהצלחה");

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
        public async Task<IActionResult> GetCurrentUser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("❌ לא נמצאה ת\"ז ב-Claim");
                return Unauthorized("Missing user ID from NameIdentifier");
            }

            Console.WriteLine($"✅ ת\"ז שנשלפה מה-Claim: {userId}");

            var user = await _registrationService.GetByIdAsync(userId);

            if (user == null)
            {
                Console.WriteLine("❌ משתמש לא נמצא במסד");
                return NotFound("User not found");
            }

            Console.WriteLine($"✅ משתמש נטען: {user.FirstName} {user.LastName}");

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
