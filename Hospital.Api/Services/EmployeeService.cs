using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Core.Interfaces.Services;

namespace Hospital.Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeService(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepo.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepo.GetByIdAsync(id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            return await _employeeRepo.AddAsync(employee);
        }

        public async Task<bool> UpdateAsync(int id, Employee employee)
        {
            var existing = await _employeeRepo.GetByIdAsync(id);
            if (existing == null) return false;

            employee.Id = id;
            await _employeeRepo.UpdateAsync(employee);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _employeeRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _employeeRepo.DeleteAsync(existing);
            return true;
        }

        // ---------------------------------------------------------
        // SEARCH employees: name, email, phone, role
        // ---------------------------------------------------------
        public async Task<IEnumerable<Employee>> SearchAsync(string term)
        {
            return await _employeeRepo.SearchAsync(term);
        }

        // ---------------------------------------------------------
        // PAGINATION + optional search + role filter + hire date range
        // ---------------------------------------------------------
        public async Task<PagedResult<Employee>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            string? role = null,
            DateTime? hiredAfter = null,
            DateTime? hiredBefore = null)
        {
            return await _employeeRepo.GetPagedAsync(
                page,
                pageSize,
                search,
                role,
                hiredAfter,
                hiredBefore
            );
        }
    }
}
