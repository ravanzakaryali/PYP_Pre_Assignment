namespace Core.Abstracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICountryRepository CountryRepository { get; }
        IProductRepository ProductRepository { get; }
        ISegmentRepository SegmentRepository { get; }
    }
}
