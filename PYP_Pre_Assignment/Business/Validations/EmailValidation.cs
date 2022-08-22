using Business.DTOs.Reports;
using FluentValidation;
using FluentValidation.Validators;

namespace Business.Validations
{
    public class EmailValidation : AbstractValidator<EmailDto>
    {
        public EmailValidation()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Please enter an email address")
                .Matches(@"^[a-zA-Z0-9]+@code.edu.az$")
                .WithMessage("The email address should only be with code.edu.az");
            }
    }
}
