using System.Linq.Expressions;

namespace Core.Abstracts.Base
{
    public interface IRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool tracking = true,
            params string[] includes);
        Task<List<TResult>> GetAllWithSelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> predicate = null,
            bool tracking = true,
            params string[] includes);
        Task<List<TEntity>> GetAllAsync<TOrderBy>(
            int page,
            int size,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            Expression<Func<TEntity, bool>> predicate = null,
            bool isOrderBy = true,
            bool tracking = true,
            params string[] includes);
        Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool tracking = true,
            params string[] include);
        Task<TResult> GetWithSelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> where = null,
            bool tracking = true,
            params string[] includes);
        Task<List<TResult>> GetAllWithSelectAsync<TOrderBy, TResult>(
            int page,
            int size,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> predicate = null,
            bool isOrderBy = true,
            bool tracking = true,
            params string[] includes);
        Task AddAsync(TEntity entity);
        TEntity Remove(TEntity entity);
        TEntity Update(TEntity entity);
        Task<int> SaveAsync();
    }
}
