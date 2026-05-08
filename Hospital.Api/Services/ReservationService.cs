using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Core.Interfaces.Services;

namespace Hospital.Api.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;

        public ReservationService(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _reservationRepo.GetAllAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _reservationRepo.GetByIdAsync(id);
        }

        public async Task<Reservation> CreateAsync(Reservation reservation)
        {
            return await _reservationRepo.AddAsync(reservation);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _reservationRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _reservationRepo.DeleteAsync(existing);
            return true;
        }

        // ---------------------------------------------------------
        // FILTERS
        // ---------------------------------------------------------
        public async Task<IEnumerable<Reservation>> GetByDoctorAsync(int doctorId)
        {
            return await _reservationRepo.GetReservationsByDoctorAsync(doctorId);
        }

        public async Task<IEnumerable<Reservation>> GetByPatientAsync(int patientId)
        {
            return await _reservationRepo.GetReservationsByPatientAsync(patientId);
        }

        // ---------------------------------------------------------
        // SEARCH
        // ---------------------------------------------------------
        public async Task<IEnumerable<Reservation>> SearchAsync(string term)
        {
            return await _reservationRepo.SearchAsync(term);
        }

        // ---------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // ---------------------------------------------------------
        public async Task<PagedResult<Reservation>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? doctorId = null,
            int? patientId = null,
            int? clinicId = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            return await _reservationRepo.GetPagedAsync(
                page, pageSize, search, doctorId, patientId, clinicId, from, to);
        }
    }
}
