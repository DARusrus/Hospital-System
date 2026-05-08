namespace Hospital.Core.DTOs.Reservation
{
    public class ReservationUpdateDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
