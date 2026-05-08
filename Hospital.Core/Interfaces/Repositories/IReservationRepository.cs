using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        // Existing endpoints
        Task<IEnumerable<Reservation>> GetReservationsByDoctorAsync(int doctorId);
        Task<IEnumerable<Reservation>> GetReservationsByPatientAsync(int patientId);

        // ---------------------------------------------
        // SEARCH: search by doctor name, patient name,
        // clinic name, status, or notes
        // ---------------------------------------------
        Task<IEnumerable<Reservation>> SearchAsync(string term);

        // ---------------------------------------------
        // PAGINATION + FILTERS + SEARCH
        // doctorId, patientId, clinicId
        // date range, status
        // ---------------------------------------------
        Task<PagedResult<Reservation>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? doctorId = null,
            int? patientId = null,
            int? clinicId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? status = null
        );
    }
}
