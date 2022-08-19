using Business.DTOs.Reports;

namespace Business.Services.Abstracts
{
    public interface IReportsService
    {
        Task<List<GetSalesByEntityDto>> SalesByCountryAsync();
        Task<List<GetSalesByEntityDto>> SalesBySegmentAsync();
        Task<List<GetSalesByEntityDto>> SalesByProductAsync();
        Task<List<GetSalesByDiscount>> SalesByDiscountAsync();
    }
}
