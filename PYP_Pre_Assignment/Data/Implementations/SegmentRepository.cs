using Core.Abstracts;
using Core.Entities;
using Data.DataAccess;
using Data.Implementations.Base;

namespace Data.Implementations
{
    public class SegmentRepository : Repository<Segment>, ISegmentRepository
    {
        public SegmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
