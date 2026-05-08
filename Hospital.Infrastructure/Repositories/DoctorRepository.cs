using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HospitalDbContext db) : base(db) { }

        // -------------------------------------------------------
        // SEARCH (Employee contains name & phone)
        // -------------------------------------------------------
        public async Task<IEnumerable<Doctor>> SearchAsync(string term)
        {
            term = term.ToLower();

            return await _db.Doctors
                .AsNoTracking()
                .Include(d => d.Employee)
                .Include(d => d.Clinic)
                .Where(d =>
                    (d.Employee.FirstName != null && d.Employee.FirstName.ToLower().Contains(term)) ||
                    (d.Employee.LastName != null && d.Employee.LastName.ToLower().Contains(term)) ||
                    (d.Specialty != null && d.Specialty.ToLower().Contains(term)) ||
                    (d.Employee.Phone != null && d.Employee.Phone.ToLower().Contains(term))
                )
                .ToListAsync();
        }

        // -------------------------------------------------------
        // PAGINATION + optional search + optional clinic filter
        // -------------------------------------------------------
        public async Task<PagedResult<Doctor>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? clinicId = null)
        {
            var query = _db.Doctors
                .AsNoTracking()
                .Include(d => d.Employee)
                .Include(d => d.Clinic)
                .AsQueryable();

            // Apply clinic filter if provided
            if (clinicId.HasValue)
            {
                query = query.Where(d => d.ClinicId == clinicId.Value);
            }

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(d =>
                    (d.Employee.FirstName != null && d.Employee.FirstName.ToLower().Contains(term)) ||
                    (d.Employee.LastName != null && d.Employee.LastName.ToLower().Contains(term)) ||
                    (d.Specialty != null && d.Specialty.ToLower().Contains(term)) ||
                    (d.Employee.Phone != null && d.Employee.Phone.ToLower().Contains(term))
                );
            }

            // Total count BEFORE pagination
            int totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .OrderBy(d => d.Employee.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Doctor>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
