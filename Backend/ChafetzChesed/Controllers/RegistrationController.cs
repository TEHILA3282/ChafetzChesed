using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChafetzChesed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _service;

        public RegistrationController(IRegistrationService service)
        {
            _service = service;
        }

        // GET: api/Registration
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetAll()
        {
            var registrations = await _service.GetAllAsync();
            return Ok(registrations);
        }

        // GET: api/Registration/123456789
        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetById(string id)
        {
            var registration = await _service.GetByIdAsync(id);
            if (registration == null)
                return NotFound("Registration not found.");
            return Ok(registration);
        }

        // POST: api/Registration
        [HttpPost]
        public async Task<ActionResult<Registration>> Add([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest("Validation failed: " + string.Join("; ", errors));
            }

            try
            {
                var added = await _service.AddAsync(registration);
                return CreatedAtAction(nameof(GetById), new { id = added.ID }, added);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception: " + ex.Message);
            }
        }

        // PUT: api/Registration
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Registration registration)
        {
            var success = await _service.UpdateAsync(registration);
            if (!success)
                return NotFound("Registration not found.");
            return NoContent();
        }

        // DELETE: api/Registration/123456789
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound("Registration not found.");
            return NoContent();
        }
    }
}
