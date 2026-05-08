using Hospital.Core.DTOs.Common;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Core.Interfaces.Services;

namespace Hospital.Api.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepo;

        public PatientService(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }

        // ---------------------------------------------------------
        // BASIC CRUD
        // ---------------------------------------------------------
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _patientRepo.GetAllAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _patientRepo.GetByIdAsync(id);
        }

        public async Task<Patient?> GetByMRNAsync(string mrn)
        {
            return await _patientRepo.GetByMedicalRecordAsync(mrn);
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            return await _patientRepo.AddAsync(patient);
        }

        public async Task<bool> UpdateAsync(int id, Patient patient)
        {
            var existing = await _patientRepo.GetByIdAsync(id);
            if (existing == null) return false;

            patient.Id = id;
            await _patientRepo.UpdateAsync(patient);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _patientRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _patientRepo.DeleteAsync(existing);
            return true;
        }

        // ---------------------------------------------------------
        // SEARCH: name, MRN, phone, email (case-insensitive)
        // ---------------------------------------------------------
        public async Task<IEnumerable<Patient>> SearchAsync(string term)
        {
            return await _patientRepo.SearchAsync(term);
        }

        // ---------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // ---------------------------------------------------------
        public async Task<PagedResult<Patient>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            string? gender = null,
            int? minAge = null,
            int? maxAge = null)
        {
            return await _patientRepo.GetPagedAsync(page, pageSize, search, gender, minAge, maxAge);
        }
    }
}
