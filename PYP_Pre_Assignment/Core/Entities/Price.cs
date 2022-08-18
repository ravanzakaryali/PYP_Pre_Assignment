using Core.Entities.Base;

namespace Core.Entities
{
    public class Price : BaseEntity<Guid>
    {
        public decimal SalePrice { get; set; }
        public decimal ManufacturingPrice { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

    }
}
