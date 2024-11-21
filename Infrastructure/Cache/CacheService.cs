using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    internal class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IConnectionMultiplexer _cache;

        private readonly IDatabase _db;
        private readonly TimeSpan DefaultExpiry;

        public CacheService(IConnectionMultiplexer cache, ILogger<CacheService> logger, IConfiguration config)
        {
            _logger = logger;
            _cache = cache;
            _db = _cache.GetDatabase();

            string? expiry = config.GetValue<string>("Cache:DefaultExpirySeconds");
            if (expiry is not null)
            {
                DefaultExpiry = TimeSpan.FromSeconds(int.Parse(expiry));
            }
            else
            {
                DefaultExpiry = TimeSpan.FromSeconds(300);
            }
        }

        public async Task<T?> Get<T>(string key)
        {
            try
            {
                RedisValue value = await _db.StringGetAsync(key);
                if (!value.HasValue)
                {
                    return default;
                }
                return JsonSerializer.Deserialize<T>(value);
            }
            catch
            {
                _logger.LogWarning("Failed to get key {@value} from cache", key);
                return default;
            }
        }

        public async Task Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                string data = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, data, expiry ?? DefaultExpiry);
            }
            catch
            {
                _logger.LogWarning("Failed to set key {@value} in cache", key);
            }
        }

        public async Task<T> GetOrCreate<T>(string key, Func<Task<T>> createItem, TimeSpan? expiry = null)
        {
            var cachedItem = await Get<T>(key);
            if (cachedItem is not null)
            {
                return cachedItem;
            }

            var newItem = await createItem();
            await Set(key, newItem, expiry ?? DefaultExpiry);
            return newItem;
        }

        public async Task Remove(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch
            {
                _logger.LogWarning("Failed to remove key {@value} from cache", key);
            }
        }

        public async Task Clear()
        {
            EndPoint[] endpoints;
            try
            {
                endpoints = _cache.GetEndPoints();
            }
            catch
            {
                _logger.LogWarning("Failed to retrieve endpoints from cache");
                return;
            }

            await Parallel.ForEachAsync(endpoints, async (endpoint, token) =>
            {
                try
                {
                    await _cache.GetServer(endpoint).FlushDatabaseAsync();
                }
                catch
                {
                    _logger.LogWarning("Failed to clear cache on endpoint {@value}", endpoint);
                }
            });  
        }
    }
}
