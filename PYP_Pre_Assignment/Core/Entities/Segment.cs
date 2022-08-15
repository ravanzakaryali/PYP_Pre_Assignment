using Core.Entities.Base;

namespace Core.Entities
{
    public class Segment : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
