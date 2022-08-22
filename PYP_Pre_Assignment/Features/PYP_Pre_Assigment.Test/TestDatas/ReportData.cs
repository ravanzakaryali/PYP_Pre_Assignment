using Business.DTOs.Reports;
using Business.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace PYP_Pre_Assigment.Test.TestDatas
{
    public class ReportData : TheoryData<SendReportDto>
    {
        public ReportData()
        {
            Add(new SendReportDto
            {
                SendEmails = new List<EmailDto>()
                {
                    new EmailDto()
                    {
                        Email = "ravan.pz@code.edu.az"
                    },
                    new EmailDto()
                    {
                        Email = "ravan.pz@code.ed.az"
                    }
                },
                Report = Report.SalesBySegment,
                EndDate = new DateTime(2014, 01, 01),
                StartDate = new DateTime(2015, 01, 01)
            });
        }
    }
}
