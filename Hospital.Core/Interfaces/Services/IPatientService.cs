using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Services
{
    public interface IPatientService
    {
        // Basic CRUD
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> GetByMRNAsync(string mrn);
        Task<Patient> CreateAsync(Patient patient);
        Task<bool> UpdateAsync(int id, Patient patient);
        Task<bool> DeleteAsync(int id);

        // Basic search: name, phone, email, MRN
        Task<IEnumerable<Patient>> SearchAsync(string term);

        // Pagination + search + optional filters
        Task<PagedResult<Patient>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            string? gender = null,
            int? minAge = null,
            int? maxAge = null
        );
    }
}
