using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Services
{
    public interface IClinicService
    {
        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        Task<IEnumerable<Clinic>> GetAllAsync();
        Task<Clinic?> GetByIdAsync(int id);
        Task<Clinic> CreateAsync(Clinic clinic);
        Task<bool> UpdateAsync(int id, Clinic clinic);
        Task<bool> DeleteAsync(int id);

        // ---------------------------------------------------------
        // SEARCH clinics: name, location, phone
        // ---------------------------------------------------------
        Task<IEnumerable<Clinic>> SearchAsync(string term);

        // ---------------------------------------------------------
        // PAGINATION + optional search
        // ---------------------------------------------------------
        Task<PagedResult<Clinic>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null
        );
    }
}
