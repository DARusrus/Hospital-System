using Hospital.Core.DTOs.Patient;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // -------------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // GET: api/patient?page=1&pageSize=10&search=ahm&gender=male&minAge=20&maxAge=60
        // -------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? gender = null,
            [FromQuery] int? minAge = null,
            [FromQuery] int? maxAge = null)
        {
            var result = await _patientService.GetPagedAsync(page, pageSize, search, gender, minAge, maxAge);
            return Ok(result);
        }

        // GET: api/patient/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        // GET: api/patient/mrn/XXXX
        [HttpGet("mrn/{mrn}")]
        public async Task<IActionResult> GetByMRN(string mrn)
        {
            var patient = await _patientService.GetByMRNAsync(mrn);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        // ----------------------------------------------------
        // SEARCH: api/patient/search?term=abc
        // ----------------------------------------------------
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term cannot be empty.");

            var results = await _patientService.SearchAsync(term);
            return Ok(results);
        }

        // POST: api/patient
        [HttpPost]
        public async Task<IActionResult> Create(PatientCreateDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                MedicalRecordNumber = dto.MedicalRecordNumber
            };

            var created = await _patientService.CreateAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/patient/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PatientUpdateDto dto)
        {
            var existing = await _patientService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            if (dto.FirstName != null) existing.FirstName = dto.FirstName;
            if (dto.LastName != null) existing.LastName = dto.LastName;
            if (dto.DateOfBirth.HasValue) existing.DateOfBirth = dto.DateOfBirth.Value;
            if (dto.Gender != null) existing.Gender = dto.Gender;
            if (dto.Phone != null) existing.Phone = dto.Phone;
            if (dto.Email != null) existing.Email = dto.Email;
            if (dto.Address != null) existing.Address = dto.Address;

            var updated = await _patientService.UpdateAsync(id, existing);
            return updated ? NoContent() : BadRequest();
        }

        // DELETE: api/patient/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _patientService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
