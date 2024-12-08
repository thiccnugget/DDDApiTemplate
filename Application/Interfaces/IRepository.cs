using Application.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> FindOneAsync(Expression<Func<T, bool>> filter, bool trackChanges = true);
        public Task<T?> FindByIdAsync(Guid id, bool trackChanges = true);

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, bool trackChanges = true);
        public Task<IEnumerable<T>> FindAsync(bool trackChanges = true);

        public Task<PagedResultDto<T>> FindPagedAsync(int page, int limit, bool trackChanges = true);
        public Task<PagedResultDto<T>> FindPagedAsync(Expression<Func<T, bool>> filter, int page, int limit, bool trackChanges = true);

        public Task<long> CountAsync(Expression<Func<T, bool>> filter);
        public Task<long> CountAsync();

        public void Add(T entity);
        public void AddRange(IEnumerable<T> entities);
        public void Update(T entity);
        public void UpdateRange(IEnumerable<T> entities);
        public void Delete(T entity);
        public void DeleteRange(IEnumerable<T> entities);
    }
}
