using Hospital.Core.Entities;
using Hospital.Core.DTOs.Common;

namespace Hospital.Core.Interfaces.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        // SEARCH — basic search (name, specialty, phone)
        Task<IEnumerable<Doctor>> SearchAsync(string term);

        // PAGINATION + optional search + optional clinic filter
        Task<PagedResult<Doctor>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? clinicId = null
        );
    }
}
