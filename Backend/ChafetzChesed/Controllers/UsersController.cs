using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.Common.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("account-actions")]
        public async Task<IActionResult> GetAccountActionsForUser()
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"CLAIM TYPE: {claim.Type} - VALUE: {claim.Value}");
            }
            var zeoutClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(zeoutClaim, out var zeout))
                return Unauthorized("Missing or invalid Zeout in token");

            var institutionIdClaim = User.Claims.FirstOrDefault(c => c.Type == "InstitutionId")?.Value;
            if (!int.TryParse(institutionIdClaim, out var institutionId))
                return Unauthorized("Missing InstitutionId");

            var actions = await _context.AccountActions
                .Where(a => a.Zeout == zeout && a.InstitutionId == institutionId)
                .OrderBy(a => a.Seder)
                .Select(a => new AccountActionDto
                {
                    Seder = a.Seder,
                    Perut = a.Perut,
                    Important = a.Important
                })
                .ToListAsync();

            return Ok(actions);
        }
    }
}
