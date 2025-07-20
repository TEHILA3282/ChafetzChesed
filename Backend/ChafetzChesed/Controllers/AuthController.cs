using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.BLL.Services;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly JwtService _jwtService;

        public AuthController(IRegistrationService registrationService, JwtService jwtService)
        {
            _registrationService = registrationService;
            _jwtService = jwtService;
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

  
            var emailMatch = users.Any(u => u.Email.ToLower() == request.Identifier.ToLower());
            var idMatch = users.Any(u => u.ID == request.Identifier);
            var passMatch = users.Any(u => u.Password == hashedPassword);
            var institutionMatch = users.Any(u => u.InstitutionId == request.InstitutionId);

            Console.WriteLine($"Email match: {emailMatch}");
            Console.WriteLine($"ID match: {idMatch}");
            Console.WriteLine($"Password match: {passMatch}");
            Console.WriteLine($"Institution match: {institutionMatch}");

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

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes); // זה חייב להיות Base64 כמו במסד!
            }
        }
    }
}
