using Application.DTOs.Paging;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
        where TEntity : BaseEntity<TEntityId>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        // compilated query to find one entity by id
        private static readonly Func<DbContext, TEntityId, Task<TEntity?>> FindEntityByIdAsync = EF.CompileAsyncQuery((DbContext context, TEntityId id) =>
            context.Set<TEntity>().FirstOrDefault(e => e.Id.Equals(id)));

        // compilated query to find one entity by id without tracking
        private static readonly Func<DbContext, TEntityId, Task<TEntity?>> FindEntityByIdNoTrackingAsync = EF.CompileAsyncQuery((DbContext context, TEntityId id) =>
            context.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Id.Equals(id)));

        public virtual async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> filter, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<TEntity?> FindByIdAsync(TEntityId id, bool trackChanges = true)
        {
            return trackChanges 
                ? await FindEntityByIdAsync(_context, id)
                : await FindEntityByIdNoTrackingAsync(_context, id);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.Where(filter).ToListAsync();
        }
        
        public virtual async Task<IEnumerable<TEntity>> FindAsync(bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<PagedResultDto<TEntity>> FindPagedAsync(
           IQueryable<TEntity> query,
           int page = 1,
           int pageSize = 10,
           bool trackChanges = true
        )
        {
            long totalItems = await query.LongCountAsync();

            IEnumerable<TEntity> items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PagedResultDto<TEntity>.Create(items, page, pageSize, totalItems);
        }

        public async Task<PagedResultDto<TResult>> FindPagedAsync<TResult>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> selector,
            int page = 1,
            int pageSize = 10,
            bool trackChanges = true
        )
        {
            long totalItems = await query.LongCountAsync();

            IEnumerable<TResult> items = await query
                .Select(selector)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PagedResultDto<TResult>.Create(items, page, pageSize, totalItems);

        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            var query = _dbSet.AsNoTracking();
            return await query.Where(filter).LongCountAsync();
        }
        
        public virtual async Task<long> CountAsync()
        {
            var query = _dbSet.AsNoTracking();
            return await query.LongCountAsync();
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
