using Azure;
using Infrastructure.Classes;
using Infrastructure.Database.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
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

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>>? expression, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
            if(expression is not null)
            {
                query.Where(expression);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<PagedResult<T>> FindPaged(Expression<Func<T, bool>> expression, int page = 1, int limit = 20)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;

            int skip = (page - 1) * limit;
            IQueryable<T> query = _dbSet.AsNoTracking().Where(expression) ;
            long totalItems = await query.LongCountAsync();
            IEnumerable<T> data = await query.Skip(skip).Take(limit).ToListAsync();

            return DataUtils.GeneratePagedResult(data, totalItems, page, limit);
        }

        public virtual async Task<PagedResult<T>> FindPaged(int page = 1, int limit = 20)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;

            int skip = (page - 1) * limit;
            IQueryable<T> query = _dbSet.AsNoTracking();
            long totalItems = await query.LongCountAsync();
            IEnumerable<T> data = await query.Skip(skip).Take(limit).ToListAsync();

            return DataUtils.GeneratePagedResult(data, totalItems, page, limit);
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

        public virtual async Task Save(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
