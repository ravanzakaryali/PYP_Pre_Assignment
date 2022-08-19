using Business.DTOs.Reports;
using FluentValidation;

namespace Business.Validations
{
    public class SendReportValidator : AbstractValidator<SendReportDto>
    {
        public SendReportValidator()
        {
            RuleFor(sr => sr.SendEmails).NotEmpty().NotNull();
            RuleFor(sr => sr.StartDate)
                .NotEmpty()
                .NotNull();
            RuleFor(sr => sr.EndDate)
                .NotEmpty()
                .NotNull();

            RuleFor(sr => sr).Must(sr => sr.EndDate > sr.StartDate)
                .WithMessage("EndTime must greater than StartTime");
        }
    }
}
