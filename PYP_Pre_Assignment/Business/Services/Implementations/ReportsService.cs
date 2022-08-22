using Business.Common;
using Business.DTOs.RabbitMq;
using Business.DTOs.Reports;
using Business.Enums;
using Business.Services.Abstracts;
using Core.Abstracts.UnitOfWork;
using MassTransit;

namespace Business.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ISendEndpointProvider _sendEndpointProvider;

        public ReportsService(IUnitOfWork unitOfWork, ISendEndpointProvider sendEndpointProvider)
        {
            _unitOfWork = unitOfWork;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task<SendReportMq> SendReport(SendReportDto reportDto)
        {
            SendReportMq sendReport = new()
            {
                Emails = reportDto.SendEmails,
            };
            switch (reportDto.Report)
            {
                case Report.SalesByCountry:
                    var salesByCountryReport = await _unitOfWork.SaleTransactionRepository.GetAllWithSelectAsync(c => (c.CountryId),
                        s => s.Date > reportDto.EndDate && s.Date < reportDto.StartDate, false);
                    List<GetSalesByEntityDto> reports = await _unitOfWork.CountryRepository.GetAllWithSelectAsync(select: c => new GetSalesByEntityDto
                    {
                        Name = c.Name,
                        Discounts = c.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                        GrosSales = c.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                        UnitsSold = c.SaleTransactions.Sum(s => s.UnitSold),
                        Profit = c.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
                    }, predicate: s => salesByCountryReport.Contains(s.Id), tracking: false, "SaleTransactions", "SaleTransactions.Price");
                    sendReport.ReportEntity = reports;
                    sendReport.Name = "sales_by_country";
                    break;
                case Report.SalesBySegment:
                    var salesSegment = await _unitOfWork.SaleTransactionRepository.GetAllWithSelectAsync(s => (s.SegmentId), s => s.Date < reportDto.EndDate && s.Date > reportDto.StartDate, false);
                    List<GetSalesByEntityDto> reportsBySegment = await _unitOfWork.SegmentRepository.GetAllWithSelectAsync(select: s => new GetSalesByEntityDto
                    {
                        Name = s.Name,
                        Discounts = s.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                        GrosSales = s.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                        UnitsSold = s.SaleTransactions.Sum(s => s.UnitSold),
                        Profit = s.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
                    }, predicate: s => salesSegment.Contains(s.Id), tracking: false, "SaleTransactions", "SaleTransactions.Price");
                    sendReport.ReportEntity = reportsBySegment;
                    sendReport.Name = "sales_by_segment";
                    break;
                case Report.SalesByProduct:
                    var salesProduct = await _unitOfWork.SaleTransactionRepository.GetAllWithSelectAsync(s => (s.ProductId), s => s.Date < reportDto.EndDate && s.Date > reportDto.StartDate, false);
                    List<GetSalesByEntityDto> reportsByProduct = await _unitOfWork.ProductRepository.GetAllWithSelectAsync(select: p => new GetSalesByEntityDto
                    {
                        Name = p.Name,
                        Discounts = p.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                        GrosSales = p.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                        UnitsSold = p.SaleTransactions.Sum(s => s.UnitSold),
                        Profit = p.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
                    }, predicate: p => salesProduct.Contains(p.Id), tracking: false, "SaleTransactions", "Prices");
                    sendReport.ReportEntity = reportsByProduct;
                    sendReport.Name = "sales_by_product";
                    break;
                case Report.SalesByDiscount:
                    var salesDiscount = await _unitOfWork.SaleTransactionRepository.GetAllWithSelectAsync(s => (s.ProductId), s => s.Date < reportDto.EndDate && s.Date > reportDto.StartDate, false);
                    List<GetSalesByDiscount> reportsByDiscount = await _unitOfWork.ProductRepository.GetAllWithSelectAsync(select: p => new GetSalesByDiscount
                    {
                        Name = p.Name,
                        Discounts = p.SaleTransactions.Average(p => p.Discounts),
                    }, predicate: p => salesDiscount.Contains(p.Id), tracking: false, "SaleTransactions", "Prices");
                    sendReport.ReportDiscount = reportsByDiscount;
                    sendReport.Name = "sales_by_dicsount";
                    break;
                default:
                    throw new Exception("Query Not found");

            }
            ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqConstants.SendReportQueue}"));
            await sendEndpoint.Send(sendReport);
            return sendReport;
        }
    }
}
