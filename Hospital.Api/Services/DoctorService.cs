using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Core.Interfaces.Services;

namespace Hospital.Api.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorService(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }

        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _doctorRepo.GetAllAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _doctorRepo.GetByIdAsync(id);
        }

        public async Task<Doctor> CreateAsync(Doctor doctor)
        {
            return await _doctorRepo.AddAsync(doctor);
        }

        public async Task<bool> UpdateAsync(int id, Doctor doctor)
        {
            var existing = await _doctorRepo.GetByIdAsync(id);
            if (existing == null) return false;

            doctor.Id = id;
            await _doctorRepo.UpdateAsync(doctor);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _doctorRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _doctorRepo.DeleteAsync(existing);
            return true;
        }

        // ---------------------------------------------------------
        // SEARCH: name, specialty, phone (case-insensitive)
        // ---------------------------------------------------------
        public async Task<IEnumerable<Doctor>> SearchAsync(string term)
        {
            return await _doctorRepo.SearchAsync(term);
        }

        // ---------------------------------------------------------
        // PAGINATION + optional clinic filter + optional search
        // ---------------------------------------------------------
        public async Task<PagedResult<Doctor>> GetPagedAsync(int? clinicId, int page, int pageSize, string? search)
        {
            return await _doctorRepo.GetPagedAsync(page, pageSize, search, clinicId);
        }
    }
}
