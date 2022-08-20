using Business.DTOs.RabbitMq;
using Business.DTOs.Reports;

namespace Business.Services.Abstracts
{
    public interface IReportsService
    {
        Task<SendReportMq> SendReport(SendReportDto reportDto);
    }
}
