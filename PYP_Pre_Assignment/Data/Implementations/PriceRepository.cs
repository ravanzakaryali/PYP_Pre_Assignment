using Core.Abstracts;
using Core.Entities;
using Data.DataAccess;
using Data.Implementations.Base;

namespace Data.Implementations
{
    public class PriceRepository : Repository<Price>, IPriceRepository
    {
        public PriceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
