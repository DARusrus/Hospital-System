using FluentValidation;
using Hospital.Core.DTOs.Reservation;

namespace Hospital.Core.Validators.Reservation
{
    public class ReservationCreateValidator : AbstractValidator<ReservationCreateDto>
    {
        public ReservationCreateValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0);

            RuleFor(x => x.PatientId)
                .GreaterThan(0);

            RuleFor(x => x.ClinicId)
                .GreaterThan(0);

            RuleFor(x => x.StartTime)
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

            RuleFor(x => x.Notes)
                .MaximumLength(200);
        }
    }
}
