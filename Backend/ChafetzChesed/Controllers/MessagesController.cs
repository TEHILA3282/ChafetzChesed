using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        // מחזיר את כל ההודעות לכל המשתמשים – אפשר לסנן בהמשך לפי ClientID
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var messages = await _context.Messages
                .OrderByDescending(m => m.DateSent)
                .ToListAsync();

            return Ok(messages);
        }

        // מחזיר הודעות למשתמש מסוים לפי תעודת זהות
        [HttpGet("by-client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByClient(string clientId)
        {
            var messages = await _context.Messages
                .Where(m => m.ClientID == clientId)
                .OrderByDescending(m => m.DateSent)
                .ToListAsync();

            return Ok(messages);
        }

        // מסמן הודעה כנקראה
        [HttpPost("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var msg = await _context.Messages.FindAsync(id);
            if (msg == null)
                return NotFound();

            msg.IsRead = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
