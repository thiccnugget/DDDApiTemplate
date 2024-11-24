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

        public virtual async Task<T?> FindOne(Expression<Func<T, bool>> expression, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(expression);
        }

        public virtual async Task<T?> FindById(Guid id, bool trackChanges = true)
        {
            return await this.FindOne(x => x.Id == id, trackChanges);
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>>? expression = null, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            if(expression is not null)
            {
                query.Where(expression);
            }
            return await query.ToListAsync();
        }
        
        public virtual async Task<PagedResultDto<T>> FindPaged(Expression<Func<T, bool>> expression, int page = 1, int limit = 20)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;

            int skip = (page - 1) * limit;
            IQueryable<T> query = _dbSet.AsNoTracking().Where(expression) ;
            long totalItems = await query.LongCountAsync();
            IEnumerable<T> data = await query.Skip(skip).Take(limit).ToListAsync();

            return new PagedResultDto<T>(data, PagedResultMetadata.Create(currentPage: page, pageSize: data.Count(), totalItems: totalItems));
        }

        public virtual async Task<PagedResultDto<T>> FindPaged(int page = 1, int limit = 20)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;

            int skip = (page - 1) * limit;
            IQueryable<T> query = _dbSet.AsNoTracking();
            long totalItems = await query.LongCountAsync();
            IEnumerable<T> data = await query.Skip(skip).Take(limit).ToListAsync();

            return new PagedResultDto<T>(data, PagedResultMetadata.Create(currentPage: page, pageSize: data.Count(), totalItems: totalItems));
        }

        public virtual async Task<long> Count(Expression<Func<T, bool>>? expression)
        {
            var query = _dbSet.AsNoTracking();
            if (expression is not null)
            {
                query.Where(expression);
            }
            return await query.LongCountAsync();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
