using Application.DTOs.Paging;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IRepository<TEntity, TEntityId> where TEntity : BaseEntity<TEntityId>
    {
        public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> filter, bool trackChanges = true);
        public Task<TEntity?> FindByIdAsync(TEntityId id, bool trackChanges = true);

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, bool trackChanges = true);
        public Task<IEnumerable<TEntity>> FindAsync(bool trackChanges = true);

        Task<PagedResultDto<TEntity>> FindPagedAsync(IQueryable<TEntity> query, int page = 1, int pageSize = 10, bool trackChanges = true);

        Task<PagedResultDto<TResult>> FindPagedAsync<TResult>(IQueryable<TEntity> query, Expression<Func<TEntity, TResult>> selector, int page = 1, int pageSize = 10, bool trackChanges = true);

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
        public Task<long> CountAsync();

        public void Add(TEntity entity);
        public void AddRange(IEnumerable<TEntity> entities);
        public void Update(TEntity entity);
        public void UpdateRange(IEnumerable<TEntity> entities);
        public void Delete(TEntity entity);
        public void DeleteRange(IEnumerable<TEntity> entities);
    }
}
