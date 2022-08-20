﻿using Business.Enums;

namespace Business.DTOs.Reports
{
    public class SendReportDto
    {
        public Report Report { get; set; }
        public List<EmailDto> SendEmails { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}