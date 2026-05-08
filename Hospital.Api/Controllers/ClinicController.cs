using Hospital.Core.DTOs.Clinic;
using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        // ----------------------------------------------------
        // PAGED LIST + SEARCH (Main Listing Endpoint)
        // GET: api/clinic?page=1&pageSize=10&search=abc
        // ----------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("page and pageSize must be greater than zero.");

            var result = await _clinicService.GetPagedAsync(page, pageSize, search);
            return Ok(result);
        }

        // ----------------------------------------------------
        // GET SINGLE CLINIC
        // ----------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic == null)
                return NotFound($"Clinic with ID {id} was not found.");

            return Ok(clinic);
        }

        // ----------------------------------------------------
        // BASIC SEARCH
        // GET: api/clinic/search?term=abc
        // ----------------------------------------------------
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term cannot be empty.");

            var results = await _clinicService.SearchAsync(term);
            return Ok(results);
        }

        // ----------------------------------------------------
        // CREATE
        // ----------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create(ClinicCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Clinic name is required.");

            var clinic = new Clinic
            {
                Name = dto.Name,
                Location = dto.Location
            };

            var created = await _clinicService.CreateAsync(clinic);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ----------------------------------------------------
        // UPDATE
        // ----------------------------------------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ClinicUpdateDto dto)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic == null)
                return NotFound($"Clinic with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(dto.Name))
                clinic.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Location))
                clinic.Location = dto.Location;

            var updated = await _clinicService.UpdateAsync(id, clinic);

            return updated ? NoContent() : BadRequest("Update failed.");
        }

        // ----------------------------------------------------
        // DELETE
        // ----------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _clinicService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound($"Clinic with ID {id} not found.");
        }
    }
}
