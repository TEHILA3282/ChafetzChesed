using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.Common.DTOs;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.Common.Utilities;
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
        private Registration? GetCurrentUser() => HttpContext.Items["User"] as Registration;

        [HttpGet("account-actions")]
        public async Task<IActionResult> GetAccountActionsForUser()
        {
            var user = GetCurrentUser();
            if (user == null)
                return Unauthorized("משתמש לא מאומת");

            var zeout = user.ID;
            var institutionId = user.InstitutionId;

            var actions = await _context.AccountActions
                .Where(a =>
                    a.InstitutionId == institutionId &&
                    a.Zeout == zeout &&
                    !( 
                        (a.Seder == 1 && (a.Important == 0 || a.Important == 1)) || 
                        (a.Seder == 7 && (a.Important == 0 || a.Important == 1))    
                    )
                )
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
        [HttpGet("account-actions/summary/{userId}")]
        public async Task<IActionResult> GetAccountSummaryByUserId(string userId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null || currentUser.Role != "Admin")
                return Unauthorized("גישה נדחתה");

            var institutionId = currentUser.InstitutionId;

            var actions = await _context.AccountActions
                .Where(a =>
                    a.InstitutionId == institutionId &&
                    a.Zeout == userId &&
                    !(
                        (a.Seder == 1 && (a.Important == 0 || a.Important == 1)) ||
                        (a.Seder == 7 && (a.Important == 0 || a.Important == 1))
                    )
                )
                .ToListAsync();

            decimal totalLoans = 0;
            decimal totalRepayments = 0;
            decimal totalDeposits = 0;
            decimal totalDonations = 0;

            foreach (var a in actions)
            {
                var perut = a.Perut?.Trim() ?? "";
                var amount = TextParsingHelper.ExtractAmount(perut);

                if (a.Important == 2 || perut.Contains("תרומה"))
                    totalDonations += amount;
                else if (a.Important == 3 || perut.Contains("הפקדה"))
                    totalDeposits += amount;
                else if (a.Important == 5 || perut.Contains("קבלת הלוו") || perut.Contains("יתרת הלוא"))
                    totalLoans += amount;
                else if (a.Important == 4 || perut.Contains("נפרע") || perut.Contains("החזר"))
                    totalRepayments += amount;
            }

            var summary = new
            {
                totalLoans,
                totalRepayments,
                totalDeposits,
                totalDonations
            };

            return Ok(summary);
        }

        [HttpGet("messages")]
        [Authorize]
        public IActionResult GetMessages()
        {
            var user = GetCurrentUser();
            if (user == null)
                return Unauthorized("משתמש לא מחובר");

            var messages = _context.Messages
                .Where(m =>
                    m.InstitutionId == user.InstitutionId &&
                    (
                        (m.Seder == 1 && (m.Zeout == null || m.Zeout == "" || m.Zeout == "0")) ||
                        (m.Seder == 7 && m.Zeout == user.ID)
                    )
                )
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return Ok(messages);
        }
        [HttpGet("account-summary")]
        public async Task<IActionResult> GetAccountSummary()
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized();

            var actions = await _context.AccountActions
                .Where(a => a.Zeout == user.ID && a.InstitutionId == user.InstitutionId)
                .ToListAsync();

            decimal totalLoans = 0;
            decimal totalRepayments = 0;
            decimal totalDeposits = 0;
            decimal totalDonations = 0;

            foreach (var a in actions)
            {
                var perut = a.Perut?.Trim() ?? "";
                var amount = TextParsingHelper.ExtractAmount(perut);

                if (a.Important == 2 || perut.Contains("תרומה"))
                {
                    totalDonations += amount;
                }
                else if (a.Important == 3 || perut.Contains("הפקדה"))
                {
                    totalDeposits += amount;
                }
                else if (a.Important == 5 || perut.Contains("קבלת הלוו") || perut.Contains("יתרת הלוא"))
                {
                    totalLoans += amount;
                }
                else if (a.Important == 4 || perut.Contains("נפרע") || perut.Contains("החזר"))
                {
                    totalRepayments += amount;
                }
            }

            var summary = new
            {
                totalLoans,
                totalRepayments,
                totalDeposits,
                totalDonations
            };

            return Ok(summary);
        }



    }
}