using Business.DTOs.Reports;

namespace Business.Services.Abstracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, List<EmailDto> emails, string attachmentPath,string fileName);
    }
}
