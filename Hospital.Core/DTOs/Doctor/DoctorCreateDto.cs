namespace Hospital.Core.DTOs.Doctor
{
    public class DoctorCreateDto
    {
        public int EmployeeId { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public int? ClinicId { get; set; }
    }
}
