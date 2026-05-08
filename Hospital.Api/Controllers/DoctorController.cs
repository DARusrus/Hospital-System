using Hospital.Core.DTOs.Common;
using Hospital.Core.DTOs.Doctor;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // -------------------------------------------------------
        // OPTIONAL FILTER + PAGINATION + SEARCH
        // GET: api/doctor?clinicId=1&page=1&pageSize=10&search=cardio
        // -------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int? clinicId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _doctorService.GetPagedAsync(clinicId, page, pageSize, search);
            return Ok(result);
        }

        // GET: api/doctor/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();

            return Ok(doctor);
        }

        // -----------------------------------------------------
        // SEARCH: api/doctor/search?term=cardio
        // -----------------------------------------------------
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term cannot be empty.");

            var results = await _doctorService.SearchAsync(term);
            return Ok(results);
        }

        // POST: api/doctor
        [HttpPost]
        public async Task<IActionResult> Create(DoctorCreateDto dto)
        {
            var doctor = new Doctor
            {
                EmployeeId = dto.EmployeeId,
                Specialty = dto.Specialty,
                ClinicId = dto.ClinicId
            };

            var created = await _doctorService.CreateAsync(doctor);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/doctor/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, DoctorUpdateDto dto)
        {
            var existing = await _doctorService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (dto.Specialty != null)
                existing.Specialty = dto.Specialty;

            if (dto.ClinicId.HasValue)
                existing.ClinicId = dto.ClinicId.Value;

            var updated = await _doctorService.UpdateAsync(id, existing);
            return updated ? NoContent() : BadRequest();
        }

        // DELETE: api/doctor/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _doctorService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
