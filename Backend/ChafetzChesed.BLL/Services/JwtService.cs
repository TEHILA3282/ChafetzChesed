using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChafetzChesed.Common; // בשביל המחלקה JwtSettings

namespace ChafetzChesed.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _settings;

       
        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        // יוצר טוקן עם ת"ז ודוא"ל
        public string GenerateToken(string id, string email)
        {
            // 1. הגדרת תביעות (Claims) – מה יהיה בתוך ה־Token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),         // ת"ז או מזהה ייחודי
                new Claim(JwtRegisteredClaimNames.Email, email),    // אימייל
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // מזהה ייחודי של ה־JWT
            };

            // 2. יצירת מפתח חתימה מה־Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. בניית ה־JWT
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
                signingCredentials: creds
            );

            // 4. הפיכת ה־Token למחרוזת (string)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
