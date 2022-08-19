namespace Business.DTOs.Reports
{
    public class GetSalesByEntityDto
    {
        public string Name { get; set; }
        public decimal UnitsSold { get; set; }
        public decimal GrosSales { get; set; }
        public decimal Discounts { get; set; }
        public decimal Profit { get; set; }
    }
}
