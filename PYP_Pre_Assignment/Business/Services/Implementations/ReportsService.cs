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
        public async Task<List<GetSalesByEntityDto>> SalesByCountryAsync()
        {
            List<GetSalesByEntityDto> reports = await _unitOfWork.CountryRepository.GetAllWithSelectAsync(select: c => new GetSalesByEntityDto
            {
                Name = c.Name,
                Discounts = c.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                GrosSales = c.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                UnitsSold = c.SaleTransactions.Sum(s => s.UnitSold),
                Profit = c.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
            }, predicate: null, tracking: false, "SaleTransactions", "SaleTransactions.Price");
            SendReportMq sendReport = new()
            {
                Name = "sales_by_country",
                ReportEntity = reports,
                ReportDiscount = null
            };
            await SendReportRabbitEntityMqAsync(sendReport);
            return reports;
        }
        public async Task<List<GetSalesByEntityDto>> SalesBySegmentAsync()
        {
            List<GetSalesByEntityDto> reports = await _unitOfWork.SegmentRepository.GetAllWithSelectAsync(select: s => new GetSalesByEntityDto
            {
                Name = s.Name,
                Discounts = s.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                GrosSales = s.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                UnitsSold = s.SaleTransactions.Sum(s => s.UnitSold),
                Profit = s.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
            }, predicate: null, tracking: false, "SaleTransactions", "SaleTransactions.Price");
            SendReportMq sendReport = new()
            {
                Name = "sales_by_segment",
                ReportEntity = reports,
                ReportDiscount = null
            };
            await SendReportRabbitEntityMqAsync(sendReport);
            return reports;
        }
        public async Task<List<GetSalesByEntityDto>> SalesByProductAsync()
        {
            List<GetSalesByEntityDto> reports = await _unitOfWork.ProductRepository.GetAllWithSelectAsync(select: p => new GetSalesByEntityDto
            {
                Name = p.Name,
                Discounts = p.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                GrosSales = p.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                UnitsSold = p.SaleTransactions.Sum(s => s.UnitSold),
                Profit = p.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
            }, predicate: null, tracking: false, "SaleTransactions", "Prices");
            SendReportMq sendReport = new()
            {
                Name = "sales_by_product",
                ReportEntity = reports,
                ReportDiscount = null,

            };
            await SendReportRabbitEntityMqAsync(sendReport);
            return reports;
        }
        public async Task<List<GetSalesByDiscount>> SalesByDiscountAsync()
        {
            List<GetSalesByDiscount> reports = await _unitOfWork.ProductRepository.GetAllWithSelectAsync(select: p => new GetSalesByDiscount
            {
                Name = p.Name,
                Discounts = p.SaleTransactions.Average(p => p.Discounts),
            }, predicate: null, tracking: false, "SaleTransactions", "Prices");
            SendReportMq sendReport = new()
            {
                Name = "sales_by_discount",
                ReportEntity = null,
                ReportDiscount = reports
            };
            await SendReportRabbitEntityMqAsync(sendReport);
            return reports;
        }
        private async Task SendReportRabbitEntityMqAsync(SendReportMq reports)
        {
            ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqConstants.SendReportQueue}"));
            await sendEndpoint.Send(reports);
        }
    }
}
