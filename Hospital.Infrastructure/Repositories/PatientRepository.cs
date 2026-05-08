using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Core.DTOs.Common;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HospitalDbContext db) : base(db) { }

        public async Task<Patient?> GetByMedicalRecordAsync(string mrn)
        {
            return await _db.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.MedicalRecordNumber == mrn);
        }

        // -------------------------------------------------------
        // SEARCH: name, MRN, phone, email (case-insensitive)
        // -------------------------------------------------------
        public async Task<IEnumerable<Patient>> SearchAsync(string term)
        {
            term = term.ToLower();

            return await _db.Patients
                .AsNoTracking()
                .Where(p =>
                    (p.FirstName != null && p.FirstName.ToLower().Contains(term)) ||
                    (p.LastName != null && p.LastName.ToLower().Contains(term)) ||
                    (p.MedicalRecordNumber != null && p.MedicalRecordNumber.ToLower().Contains(term)) ||
                    (p.Phone != null && p.Phone.ToLower().Contains(term)) ||
                    (p.Email != null && p.Email.ToLower().Contains(term))
                )
                .ToListAsync();
        }


        // -------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS (gender, age range)
        // -------------------------------------------------------
        public async Task<PagedResult<Patient>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            string? gender = null,
            int? minAge = null,
            int? maxAge = null)
        {
            var query = _db.Patients.AsNoTracking().AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(p =>
                    (p.FirstName != null && p.FirstName.ToLower().Contains(term)) ||
                    (p.LastName != null && p.LastName.ToLower().Contains(term)) ||
                    (p.MedicalRecordNumber != null && p.MedicalRecordNumber.ToLower().Contains(term)) ||
                    (p.Email != null && p.Email.ToLower().Contains(term)) ||
                    (p.Phone != null && p.Phone.ToLower().Contains(term))
                );
            }

            // Gender filter
            if (!string.IsNullOrWhiteSpace(gender))
            {
                gender = gender.ToLower();
                query = query.Where(p => p.Gender.ToLower() == gender);
            }

            // Age filters
            var today = DateTime.UtcNow.Date;

            if (minAge.HasValue)
            {
                var maxDOB = today.AddYears(-minAge.Value);
                query = query.Where(p => p.DateOfBirth <= maxDOB);
            }

            if (maxAge.HasValue)
            {
                var minDOB = today.AddYears(-maxAge.Value);
                query = query.Where(p => p.DateOfBirth >= minDOB);
            }

            // Total count
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Patient>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
