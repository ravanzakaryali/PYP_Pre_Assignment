using Core.Entities.Base;

namespace Core.Entities
{
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ManufacturingPrice { get; set; }
        public Guid SegmentId { get; set; }
        public Segment Segment { get; set; }
    }
}
