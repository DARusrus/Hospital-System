using FluentValidation;
using Hospital.Core.DTOs.Clinic;

namespace Hospital.Core.Validators.Clinic
{
    public class ClinicCreateValidator : AbstractValidator<ClinicCreateDto>
    {
        public ClinicCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(x => x.Location)
                .NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}
