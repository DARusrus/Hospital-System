using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(HospitalDbContext db) : base(db) { }

        // -------------------------------------------------------
        // GET BY DOCTOR
        // -------------------------------------------------------
        public async Task<IEnumerable<Reservation>> GetReservationsByDoctorAsync(int doctorId)
        {
            return await _db.Reservations
                .AsNoTracking()
                .Where(r => r.DoctorId == doctorId)
                .Include(r => r.Doctor).ThenInclude(d => d.Employee)
                .Include(r => r.Patient)
                .Include(r => r.Clinic)
                .ToListAsync();
        }

        // -------------------------------------------------------
        // GET BY PATIENT
        // -------------------------------------------------------
        public async Task<IEnumerable<Reservation>> GetReservationsByPatientAsync(int patientId)
        {
            return await _db.Reservations
                .AsNoTracking()
                .Where(r => r.PatientId == patientId)
                .Include(r => r.Doctor).ThenInclude(d => d.Employee)
                .Include(r => r.Clinic)
                .Include(r => r.Patient)
                .ToListAsync();
        }

        // -------------------------------------------------------
        // SEARCH: doctor name, patient name, clinic name, notes, status
        // -------------------------------------------------------
        public async Task<IEnumerable<Reservation>> SearchAsync(string term)
        {
            term = term.ToLower();

            return await _db.Reservations
                .AsNoTracking()
                .Include(r => r.Doctor).ThenInclude(d => d.Employee)
                .Include(r => r.Patient)
                .Include(r => r.Clinic)
                .Where(r =>
                    (r.Doctor.Employee.FirstName != null && r.Doctor.Employee.FirstName.ToLower().Contains(term)) ||
                    (r.Doctor.Employee.LastName != null && r.Doctor.Employee.LastName.ToLower().Contains(term)) ||
                    (r.Patient.FirstName != null && r.Patient.FirstName.ToLower().Contains(term)) ||
                    (r.Patient.LastName != null && r.Patient.LastName.ToLower().Contains(term)) ||
                    (r.Clinic.Name != null && r.Clinic.Name.ToLower().Contains(term)) ||
                    (r.Status != null && r.Status.ToLower().Contains(term)) ||
                    (r.Notes != null && r.Notes.ToLower().Contains(term))
                )
                .ToListAsync();
        }

        // -------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // -------------------------------------------------------
        public async Task<PagedResult<Reservation>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? doctorId = null,
            int? patientId = null,
            int? clinicId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? status = null)
        {
            var query = _db.Reservations
                .AsNoTracking()
                .Include(r => r.Doctor).ThenInclude(d => d.Employee)
                .Include(r => r.Patient)
                .Include(r => r.Clinic)
                .AsQueryable();

            // Doctor filter
            if (doctorId.HasValue)
                query = query.Where(r => r.DoctorId == doctorId.Value);

            // Patient filter
            if (patientId.HasValue)
                query = query.Where(r => r.PatientId == patientId.Value);

            // Clinic filter
            if (clinicId.HasValue)
                query = query.Where(r => r.ClinicId == clinicId.Value);

            // Status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                var s = status.ToLower();
                query = query.Where(r => r.Status.ToLower() == s);
            }

            // Date filters
            if (startDate.HasValue)
                query = query.Where(r => r.StartTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.StartTime <= endDate.Value);

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();

                query = query.Where(r =>
                    (r.Doctor.Employee.FirstName != null && r.Doctor.Employee.FirstName.ToLower().Contains(term)) ||
                    (r.Doctor.Employee.LastName != null && r.Doctor.Employee.LastName.ToLower().Contains(term)) ||
                    (r.Patient.FirstName != null && r.Patient.FirstName.ToLower().Contains(term)) ||
                    (r.Patient.LastName != null && r.Patient.LastName.ToLower().Contains(term)) ||
                    (r.Clinic.Name != null && r.Clinic.Name.ToLower().Contains(term)) ||
                    (r.Status != null && r.Status.ToLower().Contains(term)) ||
                    (r.Notes != null && r.Notes.ToLower().Contains(term))
                );
            }

            // Total count BEFORE pagination
            var total = await query.CountAsync();

            // Apply pagination
            var items = await query
                .OrderBy(r => r.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Reservation>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
