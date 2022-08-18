using Core.Abstracts;
using Core.Abstracts.UnitOfWork;
using Data.DataAccess;

namespace Data.Implementations.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        private ISegmentRepository _segmentRepository;
        private IProductRepository _productRepository;
        private ICountryRepository _countryRepository;
        public ISegmentRepository SegmentRepository => _segmentRepository ??= new SegmentRepository(_context);
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
        public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_context);
    }
}
