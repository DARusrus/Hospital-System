namespace Hospital.Core.Entities
{
    public class Clinic
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
