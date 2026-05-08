using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HospitalDbContext db) : base(db) { }

        // -------------------------------------------------------
        // SEARCH: first/last name, email, phone, role (case-insensitive)
        // -------------------------------------------------------
        public async Task<IEnumerable<Employee>> SearchAsync(string term)
        {
            term = term.ToLower();

            return await _db.Employees
                .AsNoTracking()
                .Where(e =>
                    (e.FirstName != null && e.FirstName.ToLower().Contains(term)) ||
                    (e.LastName != null && e.LastName.ToLower().Contains(term)) ||
                    (e.Email != null && e.Email.ToLower().Contains(term)) ||
                    (e.Phone != null && e.Phone.ToLower().Contains(term)) ||
                    (e.Role != null && e.Role.ToLower().Contains(term))
                )
                .ToListAsync();
        }

        // -------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS (role, hire date window)
        // -------------------------------------------------------
        public async Task<PagedResult<Employee>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            string? role = null,
            DateTime? hiredAfter = null,
            DateTime? hiredBefore = null)
        {
            var query = _db.Employees.AsNoTracking().AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(e =>
                    (e.FirstName != null && e.FirstName.ToLower().Contains(term)) ||
                    (e.LastName != null && e.LastName.ToLower().Contains(term)) ||
                    (e.Email != null && e.Email.ToLower().Contains(term)) ||
                    (e.Phone != null && e.Phone.ToLower().Contains(term)) ||
                    (e.Role != null && e.Role.ToLower().Contains(term))
                );
            }

            // Role filter
            if (!string.IsNullOrWhiteSpace(role))
            {
                role = role.ToLower();
                query = query.Where(e => e.Role.ToLower() == role);
            }

            // Hire date filters
            if (hiredAfter.HasValue)
                query = query.Where(e => e.HireDate >= hiredAfter.Value);

            if (hiredBefore.HasValue)
                query = query.Where(e => e.HireDate <= hiredBefore.Value);

            // Total count before pagination
            var total = await query.CountAsync();

            // Apply pagination
            var items = await query
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Employee>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
