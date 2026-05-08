namespace Hospital.Core.DTOs.Reservation
{
    public class ReservationCreateDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string? Notes { get; set; }
    }
}
