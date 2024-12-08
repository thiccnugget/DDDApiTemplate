using Application.DTOs.Paging;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // compilated query to find one entity by id
        private static readonly Func<DbContext, Guid, Task<T?>> FindEntityByIdAsync = EF.CompileAsyncQuery((DbContext context, Guid id) =>
            context.Set<T>().FirstOrDefault(x => x.Id == id));

        // compilated query to find one entity by id without tracking
        private static readonly Func<DbContext, Guid, Task<T?>> FindEntityByIdNoTrackingAsync = EF.CompileAsyncQuery((DbContext context, Guid id) =>
            context.Set<T>().AsNoTracking().FirstOrDefault(x => x.Id == id));


        public virtual async Task<T?> FindOneAsync(Expression<Func<T, bool>> filter, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<T?> FindByIdAsync(Guid id, bool trackChanges = true)
        {
            return await (
                trackChanges 
                    ? FindEntityByIdAsync(_context, id)
                    : FindEntityByIdNoTrackingAsync(_context, id));
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.Where(filter).ToListAsync();
        }
        
        public virtual async Task<IEnumerable<T>> FindAsync(bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync();
        }
        
        public virtual async Task<PagedResultDto<T>> FindPagedAsync(Expression<Func<T, bool>> filter, int page = 1, int limit = 20, bool trackChanges = true)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;
            int skip = (page - 1) * limit;

            IQueryable<T> query = _dbSet.AsNoTracking().Where(filter);
            long totalItems = await query.LongCountAsync();
            IEnumerable<T> data = await query.Skip(skip).Take(limit).ToListAsync();

            return new PagedResultDto<T>(data, PagedResultMetadata.Create(currentPage: page, pageSize: data.Count(), totalItems: totalItems));
        }

        public virtual async Task<PagedResultDto<T>> FindPagedAsync(int page = 1, int limit = 20, bool trackChanges = true)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;
            int skip = (page - 1) * limit;

            IQueryable<T> query = _dbSet.AsNoTracking();
            long totalItems = await query.LongCountAsync();
            IEnumerable<T> data = await query.Skip(skip).Take(limit).ToListAsync();

            return new PagedResultDto<T>(data, PagedResultMetadata.Create(currentPage: page, pageSize: data.Count(), totalItems: totalItems));
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> filter)
        {
            var query = _dbSet.AsNoTracking();
            return await query.Where(filter).LongCountAsync();
        }
        
        public virtual async Task<long> CountAsync()
        {
            var query = _dbSet.AsNoTracking();
            return await query.LongCountAsync();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
