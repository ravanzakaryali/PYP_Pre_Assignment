using Core.Abstracts;
using Core.Entities;
using Data.DataAccess;
using Data.Implementations.Base;

namespace Data.Implementations
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
