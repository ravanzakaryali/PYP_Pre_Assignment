using Core.Entities.Base;

namespace Core.Entities
{
    public class Country : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<SaleTransaction> SaleTransactions { get; set; }
    }
}
