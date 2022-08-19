using Business.DTOs.Reports;
using Business.Enums;
using Business.Services.Abstracts;
using Core.Abstracts.UnitOfWork;

namespace Business.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        readonly IUnitOfWork _unitOfWork;

        public ReportsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetSalesByEntityDto>> SalesByCountryAsync()
        {
            return await _unitOfWork.CountryRepository.GetAllWithSelectAsync(select: c => new GetSalesByEntityDto
            {
                Name = c.Name,
                Discounts = c.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                GrosSales = c.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                UnitsSold = c.SaleTransactions.Sum(s => s.UnitSold),
                Profit = c.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
            }, predicate: null, tracking: false, "SaleTransactions", "SaleTransactions.Price");
        }
        public async Task<List<GetSalesByEntityDto>> SalesBySegmentAsync()
        {
            return await _unitOfWork.SegmentRepository.GetAllWithSelectAsync(select: s => new GetSalesByEntityDto
            {
                Name = s.Name,
                Discounts = s.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                GrosSales = s.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                UnitsSold = s.SaleTransactions.Sum(s => s.UnitSold),
                Profit = s.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
            }, predicate: null, tracking: false, "SaleTransactions", "SaleTransactions.Price");
        }
        public async Task<List<GetSalesByEntityDto>> SalesByProductAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllWithSelectAsync(select: p => new GetSalesByEntityDto
            {
                Name = p.Name,
                Discounts = p.SaleTransactions.Sum(s => (s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100),
                GrosSales = p.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold),
                UnitsSold = p.SaleTransactions.Sum(s => s.UnitSold),
                Profit = p.SaleTransactions.Sum(s => s.Price.SalePrice * s.UnitSold - ((s.Price.SalePrice * s.UnitSold) * (s.Discounts) / 100) - (s.Price.ManufacturingPrice) * s.UnitSold)
            }, predicate: null, tracking: false, "SaleTransactions", "Prices");
        }
        public async Task<List<GetSalesByDiscount>> SalesByDiscountAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllWithSelectAsync(select: p => new GetSalesByDiscount
            {
                Name = p.Name,
                Discounts = p.SaleTransactions.Average(p => p.Discounts),
            }, predicate: null, tracking: false, "SaleTransactions", "Prices");
        }
    }
}
