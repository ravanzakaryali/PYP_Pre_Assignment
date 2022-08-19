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
        private ISaleTransactionRepository _saleTransactionRepository;
        private IPriceRepository _priceRepository;
        public ISegmentRepository SegmentRepository => _segmentRepository ??= new SegmentRepository(_context);
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
        public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_context);
        public ISaleTransactionRepository SaleTransactionRepository => _saleTransactionRepository ??= new SaleTransactionRepository(_context);
        public IPriceRepository PriceRepository => _priceRepository ??= new PriceRepository(_context);


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
