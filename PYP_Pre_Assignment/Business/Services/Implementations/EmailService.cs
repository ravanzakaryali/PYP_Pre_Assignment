using Business.DTOs.Reports;
using Business.Services.Abstracts;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Business.Services.Implementations
{
    public class EmailService : IEmailService
    {
        readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string subject, List<EmailDto> emails, string attachmentPath,string fileName)
        {
            var from = new EmailAddress(_configuration["SendGrid:Email"], _configuration["SendGrid:User"]);
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            foreach (var email in emails)
            {
                var to = new EmailAddress(email.Email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                using var fileStream = File.OpenRead(attachmentPath);
                await msg.AddAttachmentAsync(fileName, fileStream);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
