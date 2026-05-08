using FluentValidation;
using Hospital.Core.DTOs.Patient;

namespace Hospital.Core.Validators.Patient
{
    public class PatientCreateValidator : AbstractValidator<PatientCreateDto>
    {
        public PatientCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g.ToLower() is "male" or "female" or "other")
                .WithMessage("Gender must be one of: male, female, other.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be a past date.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9+\- ]+$").WithMessage("Invalid phone format.");

            RuleFor(x => x.MedicalRecordNumber)
                .NotEmpty().WithMessage("MRN is required.")
                .MinimumLength(3).MaximumLength(50);
        }
    }
}
