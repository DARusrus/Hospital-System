using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class ClinicRepository : Repository<Clinic>, IClinicRepository
    {
        public ClinicRepository(HospitalDbContext db) : base(db) { }

        // -------------------------------------------------------
        // SEARCH: name, location, phone (case-insensitive)
        // -------------------------------------------------------
        public async Task<IEnumerable<Clinic>> SearchAsync(string term)
        {
            term = term.ToLower();

            return await _db.Clinics
                .AsNoTracking()
                .Where(c =>
                    (c.Name != null && c.Name.ToLower().Contains(term)) ||
                    (c.Location != null && c.Location.ToLower().Contains(term)) ||
                    (c.Phone != null && c.Phone.ToLower().Contains(term))
                )
                .ToListAsync();
        }

        // -------------------------------------------------------
        // PAGINATION + optional search
        // -------------------------------------------------------
        public async Task<PagedResult<Clinic>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null
        )
        {
            var query = _db.Clinics
                .AsNoTracking()
                .AsQueryable();

            // Apply search if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(c =>
                    (c.Name != null && c.Name.ToLower().Contains(term)) ||
                    (c.Location != null && c.Location.ToLower().Contains(term)) ||
                    (c.Phone != null && c.Phone.ToLower().Contains(term))
                );
            }

            // Total BEFORE pagination
            int total = await query.CountAsync();

            // Apply pagination
            var items = await query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Clinic>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
