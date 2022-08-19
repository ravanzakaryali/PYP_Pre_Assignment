using Core.Abstracts;
using Core.Entities;
using Data.DataAccess;
using Data.Implementations.Base;

namespace Data.Implementations
{
    public class SaleTransactionRepository : Repository<SaleTransaction>, ISaleTransactionRepository
    {
        public SaleTransactionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
