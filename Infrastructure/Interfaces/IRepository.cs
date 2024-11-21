using Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> FindOne(Expression<Func<T, bool>> expression, bool trackChanges = true);
        public Task<T?> FindById(Guid id, bool trackChanges = true);
        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>>? expression, bool trackChanges = true);
        public Task<PagedResult<T>> FindPaged(int page = 1, int limit = 20);
        public Task<PagedResult<T>> FindPaged(Expression<Func<T, bool>> expression, int page = 1, int limit = 20);
        public Task<long> Count(Expression<Func<T, bool>>? expression);
        public Task Save(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
