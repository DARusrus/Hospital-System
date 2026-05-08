using Hospital.Core.DTOs.Reservation;
using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IClinicService _clinicService;

        public ReservationController(
            IReservationService reservationService,
            IDoctorService doctorService,
            IPatientService patientService,
            IClinicService clinicService)
        {
            _reservationService = reservationService;
            _doctorService = doctorService;
            _patientService = patientService;
            _clinicService = clinicService;
        }

        // ---------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS (MAIN ENDPOINT)
        // GET api/reservation?page=1&pageSize=10&doctorId=3&search=ahmed
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? doctorId = null,
            [FromQuery] int? patientId = null,
            [FromQuery] int? clinicId = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var result = await _reservationService.GetPagedAsync(
                page, pageSize, search, doctorId, patientId, clinicId, from, to);
            
            return Ok(result);
        }

        // GET: api/reservation/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            return Ok(reservation);
        }

        // ---------------------------------------------------------
        // FILTER ONLY: doctor
        // GET: api/reservation/doctor/3
        // ---------------------------------------------------------
        [HttpGet("doctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await _reservationService.GetByDoctorAsync(doctorId);
            return Ok(result);
        }

        // FILTER ONLY: patient
        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _reservationService.GetByPatientAsync(patientId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // SEARCH (simple search)
        // GET: api/reservation/search?term=ahmed
        // ---------------------------------------------------------
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term cannot be empty.");

            var results = await _reservationService.SearchAsync(term);
            return Ok(results);
        }

        // POST: api/reservation
        [HttpPost]
        public async Task<IActionResult> Create(ReservationCreateDto dto)
        {
            // Validate Doctor
            if (await _doctorService.GetByIdAsync(dto.DoctorId) == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} does not exist.");

            // Validate Patient
            if (await _patientService.GetByIdAsync(dto.PatientId) == null)
                return BadRequest($"Patient with ID {dto.PatientId} does not exist.");

            // Validate Clinic
            if (await _clinicService.GetByIdAsync(dto.ClinicId) == null)
                return BadRequest($"Clinic with ID {dto.ClinicId} does not exist.");

            var reservation = new Reservation
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                ClinicId = dto.ClinicId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Notes = dto.Notes,
                Status = "Scheduled"
            };

            var created = await _reservationService.CreateAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // DELETE: api/reservation/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reservationService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
