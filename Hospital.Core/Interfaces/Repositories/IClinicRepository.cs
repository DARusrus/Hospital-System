using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Repositories
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        // ---------------------------------------------------------
        // BASIC SEARCH: name, location (case-insensitive)
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
