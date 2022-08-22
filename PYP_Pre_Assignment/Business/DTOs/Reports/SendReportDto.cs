using Business.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Business.DTOs.Reports
{
    public class SendReportDto
    {
        [FromRoute(Name = "type")]
        public Report Report { get; set; }
        [FromBody]
        public List<EmailDto> SendEmails { get; set; }
        [FromQuery]
        public DateTime StartDate { get; set; }
        [FromQuery]
        public DateTime EndDate { get; set; }
    }
}
