using Business.DTOs.RabbitMq;
using Business.Services.Abstracts;
using MassTransit;

namespace Business.Consumer
{
    public class ReportConsumer : IConsumer<SendReportMq>
    {
        readonly IFileService _fileService;

        public ReportConsumer(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Task Consume(ConsumeContext<SendReportMq> context)
        {
            _fileService.ObjectToExcel(context.Message.ReportEntity, $"{context.Message.Name}.xlxs");
            return Task.CompletedTask;
        }
    }
}
