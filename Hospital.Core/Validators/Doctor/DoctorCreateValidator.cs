using FluentValidation;
using Hospital.Core.DTOs.Doctor;

namespace Hospital.Core.Validators.Doctor
{
    public class DoctorCreateValidator : AbstractValidator<DoctorCreateDto>
    {
        public DoctorCreateValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0);

            RuleFor(x => x.Specialty)
                .NotEmpty().MinimumLength(2).MaximumLength(100);

            RuleFor(x => x.ClinicId)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
