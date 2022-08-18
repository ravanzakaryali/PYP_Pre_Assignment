using Core.Abstracts.Base;
using Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Implementations.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();
        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool tracking = true,
            params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            if (!tracking)
                query = Table.AsNoTracking();
            return predicate is null
                ? await query.ToListAsync()
                : await query.Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync<TOrderBy>(
            int page,
            int size, Expression<Func<TEntity, TOrderBy>> orderBy,
            Expression<Func<TEntity, bool>> where = null,
            bool isOrderBy = true,
            bool tracking = true,
            params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            if (!tracking)
                query = Table.AsNoTracking();
            return where is null
                ? isOrderBy
                    ? await query.Skip((page - 1) * size).Take(size).ToListAsync()
                    : await query.OrderByDescending(orderBy).Skip((page - 1) * size).Take(size).ToListAsync()
                : isOrderBy
                    ? await query.Where(where).Skip((page - 1) * size).Take(size).ToListAsync()
                    : await query.Where(where).OrderByDescending(orderBy).Skip((page - 1) * size).Take(size).ToListAsync();
        }

        public async Task<List<TResult>> GetAllWithSelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> where = null,
            bool tracking = true,
            params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            if (!tracking)
                query = Table.AsNoTracking();
            return where is null
                ? await query.Select(select).ToListAsync()
                : await query.Where(where).Select(select).ToListAsync();
        }

        public async Task<List<TResult>> GetAllWithSelectAsync<TOrderBy, TResult>(
            int page,
            int size,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> where = null,
            bool isOrderBy = true,
            bool tracking = true,
            params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            if (!tracking)
                query = Table.AsNoTracking();
            return where is null
                ? isOrderBy
                    ? await query.Skip((page - 1) * size).Take(size).Select(select).ToListAsync()
                    : await query.OrderByDescending(orderBy).Skip((page - 1) * size).Take(size).Select(select).ToListAsync()
                : isOrderBy
                    ? await query.Where(where).Skip((page - 1) * size).Take(size).Select(select).ToListAsync()
                    : await query.Where(where).OrderByDescending(orderBy).Skip((page - 1) * size).Take(size).Select(select).ToListAsync();
        }

        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> where = null,
            bool tracking = true,
            params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            if (!tracking)
                query = Table.AsNoTracking();
            return where is null
                ? await query.FirstOrDefaultAsync()
                : await query.Where(where).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetWithSelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> where = null,
            bool tracking = true,
            params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            if (!tracking)
                query = Table.AsNoTracking();
            return where is null
               ? await query.Select(select).FirstOrDefaultAsync()
               : await query.Where(where).Select(select).FirstOrDefaultAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public TEntity Remove(TEntity entity)
        {
            return _context.Set<TEntity>().Remove(entity).Entity;
        }
        public TEntity Update(TEntity entity)
        {
            return _context.Set<TEntity>().Update(entity).Entity;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        private IQueryable<TEntity> GetQuery(params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return query;
        }
    }
}
