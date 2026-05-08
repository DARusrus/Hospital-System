namespace Hospital.Core.DTOs.Doctor
{
    public class DoctorResponseDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public int? ClinicId { get; set; }

        // Optional enhanced data for clients (if needed in controllers)
        public string? ClinicName { get; set; }
        public string? EmployeeFullName { get; set; }
    }
}
