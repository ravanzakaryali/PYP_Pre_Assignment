using Core.Abstracts;
using Core.Entities;
using Data.DataAccess;
using Data.Implementations.Base;

namespace Data.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
