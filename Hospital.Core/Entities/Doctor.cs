namespace Hospital.Core.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }   // FK → Employee
        public Employee Employee { get; set; } = null!;

        public string Specialty { get; set; } = string.Empty;

        public int? ClinicId { get; set; }   // Doctor is assigned to a clinic (optional)
        public Clinic? Clinic { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
