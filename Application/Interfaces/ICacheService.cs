using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICacheService
    {
        public Task<T?> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        public Task RemoveAsync(string key);
        public Task<bool> ExistsAsync(string key);
        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);
    }
}
