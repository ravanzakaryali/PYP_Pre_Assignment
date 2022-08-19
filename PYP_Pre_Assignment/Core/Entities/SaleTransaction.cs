using Core.Entities.Base;

namespace Core.Entities
{
    public class SaleTransaction : BaseEntity<Guid>
    {
        public decimal UnitSold { get; set; }
        public decimal Discounts { get; set; }
        public decimal Sales { get; set; }
        public DateTime Date { get; set; }
        public Guid PriceId { get; set; }
        public Price Price { get; set; }
        public Guid ProductId { get; set; } 
        public Product Product { get; set; }
        public Guid SegmentId { get; set; }
        public Segment Segment { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
    }
}
