using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICacheService
    {
        public Task<T?> Get<T>(string key);
        public Task Set<T>(string key, T value, TimeSpan? expiry);

        public Task Remove(string key);

        public Task Clear();

        public Task<T> GetOrCreate<T>(string key, Func<Task<T>> factory, TimeSpan? expiry);
    }
}
