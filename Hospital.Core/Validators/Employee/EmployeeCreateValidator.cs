using FluentValidation;
using Hospital.Core.DTOs.Employee;

namespace Hospital.Core.Validators.Employee
{
    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty().Matches(@"^[0-9+\- ]+$");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Employee role is required.");
        }
    }
}
