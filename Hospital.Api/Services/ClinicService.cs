using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Core.Interfaces.Services;

namespace Hospital.Api.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _clinicRepo;

        public ClinicService(IClinicRepository clinicRepo)
        {
            _clinicRepo = clinicRepo;
        }

        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        public async Task<IEnumerable<Clinic>> GetAllAsync()
        {
            return await _clinicRepo.GetAllAsync();
        }

        public async Task<Clinic?> GetByIdAsync(int id)
        {
            return await _clinicRepo.GetByIdAsync(id);
        }

        public async Task<Clinic> CreateAsync(Clinic clinic)
        {
            return await _clinicRepo.AddAsync(clinic);
        }

        public async Task<bool> UpdateAsync(int id, Clinic clinic)
        {
            var existing = await _clinicRepo.GetByIdAsync(id);
            if (existing == null) return false;

            clinic.Id = id;
            await _clinicRepo.UpdateAsync(clinic);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _clinicRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _clinicRepo.DeleteAsync(existing);
            return true;
        }

        // ---------------------------------------------------------
        // SEARCH
        // ---------------------------------------------------------
        public async Task<IEnumerable<Clinic>> SearchAsync(string term)
        {
            return await _clinicRepo.SearchAsync(term);
        }

        // ---------------------------------------------------------
        // PAGINATION + optional search
        // ---------------------------------------------------------
        public async Task<PagedResult<Clinic>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            return await _clinicRepo.GetPagedAsync(page, pageSize, search);
        }
    }
}
