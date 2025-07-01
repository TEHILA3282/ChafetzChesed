using Microsoft.AspNetCore.Mvc;
using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;

        public LoansController(ILoanService service)
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
        public async Task<IActionResult> Create(Loan loan)
        {
            var created = await _service.AddAsync(loan);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Loan loan)
        {
            var updated = await _service.UpdateAsync(loan);
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