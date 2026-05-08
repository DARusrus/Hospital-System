namespace Hospital.Core.DTOs.Reservation
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }

        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }

        public int ClinicId { get; set; }
        public string? ClinicName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
