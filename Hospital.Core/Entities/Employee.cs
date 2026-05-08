namespace Hospital.Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty; // Admin, Nurse, Receptionist, etc.

        public DateTime HireDate { get; set; } = DateTime.UtcNow;
    }
}
