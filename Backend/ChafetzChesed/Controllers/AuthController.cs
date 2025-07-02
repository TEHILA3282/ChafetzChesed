using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.BLL.Services;
using ChafetzChesed.DAL.Entities;
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

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var users = await _registrationService.GetAllAsync();
            string hashedPassword = HashPassword(request.Password);

            var user = users.FirstOrDefault(u =>
                (u.Email == request.Identifier || u.ID == request.Identifier) &&
                u.Password == hashedPassword
            );

            if (user == null)
                return Unauthorized("המייל או הסיסמה שגויים.");

            string token = _jwtService.GenerateToken(user.ID, user.Email);

            return Ok(new
            {
                message = "התחברת בהצלחה!",
                token,
                user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.ID,
                    user.Email
                }
            });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // מודל לבקשת התחברות
        public class LoginRequest
        {
            public string Identifier { get; set; } = string.Empty; // מייל או ת"ז
            public string Password { get; set; } = string.Empty;
        }
    }
}
