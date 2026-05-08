using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;

namespace Hospital.Core.Interfaces.Services
{
    public interface IReservationService
    {
        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(int id);
        Task<Reservation> CreateAsync(Reservation reservation);
        Task<bool> DeleteAsync(int id);

        // ---------------------------------------------------------
        // FILTER BY DOCTOR / PATIENT
        // ---------------------------------------------------------
        Task<IEnumerable<Reservation>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<Reservation>> GetByPatientAsync(int patientId);

        // ---------------------------------------------------------
        // SEARCH reservations: doctor name, patient name, clinic, date
        // ---------------------------------------------------------
        Task<IEnumerable<Reservation>> SearchAsync(string term);

        // ---------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // doctorId, patientId, clinicId, date range
        // ---------------------------------------------------------
        Task<PagedResult<Reservation>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? doctorId = null,
            int? patientId = null,
            int? clinicId = null,
            DateTime? from = null,
            DateTime? to = null
        );
    }
}
