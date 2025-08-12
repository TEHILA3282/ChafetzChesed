using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.Common.DTOs;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepositsController : ControllerBase
    {
        private readonly IDepositService _service;

        private static readonly HashSet<string> AllowedPaymentMethods = new(new[]
        {
            "גביה מאשראי אחר",
            "גביה באשראי המופיע בקופת הגמ\"ח",
            "חתימה על הוראת קבע חדשה",
            "הו\"ק קיימת בקופת הגמ\"ח"
        });

        private static readonly Dictionary<string, string> PaymentMethodMap = new()
        {
            ["credit_other"] = "גביה מאשראי אחר",
            ["credit_existing"] = "גביה באשראי המופיע בקופת הגמ\"ח",
            ["new"] = "חתימה על הוראת קבע חדשה",
            ["existing"] = "הו\"ק קיימת בקופת הגמ\"ח"
        };

        public DepositsController(IDepositService service)
        {
            _service = service;
        }

        private Registration? GetLoggedInUser() => HttpContext.Items["User"] as Registration;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepositDto dto)
        {
            var user = GetLoggedInUser();
            if (user == null)
                return Unauthorized("משתמש לא מחובר");

            if (!dto.Amount.HasValue || dto.Amount <= 0)
                return BadRequest("סכום ההפקדה חייב להיות גדול מ-0");
            if (dto.DepositTypeID <= 0)
                return BadRequest("חובה לבחור סוג הפקדה");

            string? dbPaymentMethod = null;

            if (!string.IsNullOrWhiteSpace(dto.PaymentMethod))
            {
                if (AllowedPaymentMethods.Contains(dto.PaymentMethod))
                {
                    dbPaymentMethod = dto.PaymentMethod;
                }
                else if (PaymentMethodMap.TryGetValue(dto.PaymentMethod, out var mapped))
                {
                    dbPaymentMethod = mapped;
                }
                else
                {
                    return BadRequest($"אופן התשלום אינו חוקי: {dto.PaymentMethod}");
                }
            }

            if (dto.IsDirectDeposit && string.IsNullOrWhiteSpace(dbPaymentMethod))
                return BadRequest("בהפקדה אוטומטית חובה לבחור אופן תשלום.");

            var deposit = new Deposit
            {
                ClientID = user.ID,                          
                DepositTypeID = dto.DepositTypeID,
                Amount = dto.Amount.Value,
                PurposeDetails = dto.PurposeDetails,
                IsDirectDeposit = dto.IsDirectDeposit,
                DepositDate = dto.DepositDate ?? DateTime.UtcNow,
                DepositReceivedDate = dto.DepositReceivedDate,
                PaymentMethod = dbPaymentMethod 
            };

            try
            {
                var created = await _service.AddAsync(deposit);
                return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "שמירת ההפקדה נכשלה",
                    detail: ex.InnerException?.Message ?? ex.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Deposit deposit)
        {
            if (!string.IsNullOrWhiteSpace(deposit.PaymentMethod) &&
                !AllowedPaymentMethods.Contains(deposit.PaymentMethod))
            {
                return BadRequest($"אופן התשלום חייב להיות אחד מהבאים: {string.Join(", ", AllowedPaymentMethods)}");
            }

            var updated = await _service.UpdateAsync(deposit);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
