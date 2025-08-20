using Microsoft.AspNetCore.Mvc;
using ChafetzChesed.Common.DTOs;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _db;
    public ContactController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactRequestCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Subject) ||
            string.IsNullOrWhiteSpace(dto.Message))
        {
            return BadRequest("יש למלא את כל השדות");
        }

        var entity = new ContactRequest
        {
            InstitutionId = dto.InstitutionId,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim(),
            Subject = dto.Subject.Trim(),
            Message = dto.Message.Trim()
        };

        _db.ContactRequests.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(new { id = entity.ID });
    }
}
