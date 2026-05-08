using Hospital.Core.Entities;
using Hospital.Core.DTOs.Common;

namespace Hospital.Core.Interfaces.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetByMedicalRecordAsync(string mrn);

        // Search patients by name, MRN, phone, or email
        Task<IEnumerable<Patient>> SearchAsync(string term);

        // Pagination + search + filters
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
