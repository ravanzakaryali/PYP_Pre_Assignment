using Core.Entities.Base;

namespace Core.Entities
{
    public class Sale : BaseEntity<Guid>
    {
        public DateTime Date { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public double UnitsSale { get; set; }
        public double? Discount { get; set; } //%
}
}
