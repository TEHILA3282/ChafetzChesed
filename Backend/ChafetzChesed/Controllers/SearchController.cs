using ChafetzChesed.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace ChafetzChesed.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{term}")]
        public async Task<IActionResult> Search(string term)
        {
            term = term.ToLower();

            var actions = await _context.AccountActions
                .Where(a => a.Perut.ToLower().Contains(term))
                .Select(a => new { title = a.Perut, route = "/account", type = "פעולה" })
                .ToListAsync();

            var deposits = await _context.Deposits
                .Where(d => d.PurposeDetails != null && d.PurposeDetails.ToLower().Contains(term))
                .Select(d => new { title = d.PurposeDetails, route = "/actions/deposit", type = "הפקדה" })
                .ToListAsync();


            return Ok();
        }
    }
}