using Business.DTOs.Reports;
using Business.Enums;
using Business.Services.Abstracts;

namespace Business.Services.Implementations
{
    public class ReportsService : IReportsService    
    {
        public void SendReportsAsync(SendReportDto reportDto)
        {
            Report reportType = reportDto.Report;
            switch (reportType)
            {
                case Report.SalesByCountry:
                    break;
                case Report.SalesBySegment:
                    break;
                case Report.SalesByProduct:
                    break;
                case Report.SalesByDiscount:
                    break;
                default:
                    break;
            }
        }
    }
}
