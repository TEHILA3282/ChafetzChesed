using Microsoft.AspNetCore.Mvc;
using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositsController : ControllerBase
    {
        private readonly IDepositService _service;

        public DepositsController(IDepositService service)
        {
            _service = service;
        }

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
        public async Task<IActionResult> Create(Deposit deposit)
        {
            var created = await _service.AddAsync(deposit);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Deposit deposit)
        {
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