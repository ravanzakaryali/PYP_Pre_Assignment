using Core.Entities.Base;

namespace Core.Entities
{
    public class Country : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
