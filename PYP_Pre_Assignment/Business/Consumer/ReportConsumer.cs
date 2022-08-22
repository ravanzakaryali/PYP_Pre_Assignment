using Business.DTOs.RabbitMq;
using Business.Services.Abstracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Business.Consumer
{
    public class ReportConsumer : IConsumer<SendReportMq>
    {
        readonly IFileService _fileService;
        readonly IEmailService _emailService;
        readonly ILogger _logger;

        public ReportConsumer(IFileService fileService, IEmailService emailService, ILogger logger)
        {
            _fileService = fileService;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<SendReportMq> context)
        {
            string root = String.Empty;
            if (context.Message.ReportEntity is not null)
                root = _fileService.ObjectToExcel(context.Message.ReportEntity, $"{context.Message.Name}.xlsx");
            if (context.Message.ReportDiscount is not null)
                root = _fileService.ObjectToExcel(context.Message.ReportDiscount, $"{context.Message.Name}.xlsx");
            await _emailService.SendEmailAsync(context.Message.Name, context.Message.Emails, root, $"{context.Message.Name}.xlsx");
            _logger.LogError($"Email {context.Message.Emails} report {context.Message.Name}", context.Message.Name);
        }
    }
}
