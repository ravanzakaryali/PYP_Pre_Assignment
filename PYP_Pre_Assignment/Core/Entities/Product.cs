using Core.Entities.Base;

namespace Core.Entities
{
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<SaleTransaction> SaleTransactions { get; set; }
        public ICollection<Price> Prices { get; set; }
    }
}
