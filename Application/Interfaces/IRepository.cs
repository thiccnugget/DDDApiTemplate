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
        public Task<T?> FindOne(Expression<Func<T, bool>> expression, bool trackChanges = true);
        public Task<T?> FindById(Guid id, bool trackChanges = true);
        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>>? expression = null, bool trackChanges = true);
        public Task<PagedResultDto<T>> FindPaged(int page = 1, int limit = 20);
        public Task<PagedResultDto<T>> FindPaged(Expression<Func<T, bool>> expression, int page = 1, int limit = 20);
        public Task<long> Count(Expression<Func<T, bool>>? expression);
        public void Add(T entity);
        public void AddRange(IEnumerable<T> entities);
        public void Update(T entity);
        public void UpdateRange(IEnumerable<T> entities);
        public void Delete(T entity);
        public void DeleteRange(IEnumerable<T> entities);

    }
}
