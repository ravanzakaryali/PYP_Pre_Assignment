using Business.DTOs.Reports;

namespace Business.DTOs.RabbitMq
{
    public class SendReportMq
    {
        public string Name { get; set; }
        public List<GetSalesByDiscount> ReportDiscount { get; set; }
        public List<GetSalesByEntityDto> ReportEntity { get; set; }
        public List<EmailDto> Emails { get; set; }
    }
}
