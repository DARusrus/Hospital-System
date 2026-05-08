using Hospital.Core.Entities;
using Hospital.Core.DTOs.Common;

namespace Hospital.Core.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // -------------------------------------------------------
        // Search employees by name, phone, email, or role
        // -------------------------------------------------------
        Task<IEnumerable<Employee>> SearchAsync(string term);

        // -------------------------------------------------------
        // Pagination + optional search + optional filters
        // -------------------------------------------------------
        Task<PagedResult<Employee>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            string? role = null,
            DateTime? hiredAfter = null,
            DateTime? hiredBefore = null
        );
    }
}
