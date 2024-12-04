using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICacheService
    {
        public Task<T?> Get<T>(string key);
        public Task Set<T>(string key, T value, TimeSpan? expiry = null);
        public Task Remove(string key);
        public Task<bool> Exists(string key);
        public Task<T> GetOrCreate<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);
    }
}
