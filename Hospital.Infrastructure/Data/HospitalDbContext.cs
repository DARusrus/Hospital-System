using Hospital.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
            : base(options)
        {
        }

        // DbSets (tables)
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // EMPLOYEE 1 → 1 DOCTOR (OPTION C - FK relation)
            // =============================================
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Employee)
                .WithMany() // no navigation back
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================================
            // CLINIC 1 → MANY DOCTORS
            // =============================================
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Clinic)
                .WithMany(c => c.Doctors)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.SetNull);

            // =============================================
            // CLINIC 1 → MANY RESERVATIONS
            // =============================================
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Clinic)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================================
            // DOCTOR 1 → MANY RESERVATIONS
            // =============================================
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Reservations)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================================
            // PATIENT 1 → MANY RESERVATIONS
            // =============================================
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.Reservations)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================================
            // UNIQUE: PATIENT.MedicalRecordNumber
            // =============================================
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.MedicalRecordNumber)
                .IsUnique();
        }
    }
}
