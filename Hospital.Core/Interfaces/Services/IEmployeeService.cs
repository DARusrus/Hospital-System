using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<bool> UpdateAsync(int id, Employee employee);
        Task<bool> DeleteAsync(int id);

        // ---------------------------------------------------------
        // SEARCH employees: name, email, phone, role
        // ---------------------------------------------------------
        Task<IEnumerable<Employee>> SearchAsync(string term);

        // ---------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // role, hire-date range
        // ---------------------------------------------------------
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
