using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Services
{
    public interface IDoctorService
    {
        // Basic CRUD
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor> CreateAsync(Doctor doctor);
        Task<bool> UpdateAsync(int id, Doctor doctor);
        Task<bool> DeleteAsync(int id);

        // Search doctors by name, specialty, or phone
        Task<IEnumerable<Doctor>> SearchAsync(string term);

        // Pagination + optional clinic filter + optional search
        Task<PagedResult<Doctor>> GetPagedAsync(int? clinicId, int page, int pageSize, string? search);
    }
}
