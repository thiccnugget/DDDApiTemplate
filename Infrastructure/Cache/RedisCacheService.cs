using Application.Interfaces;
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
    internal class RedisCacheService : ICacheService
    {
        private readonly ILogger<RedisCacheService> _logger;
        private readonly IDatabase _db;
        private readonly TimeSpan DefaultExpiry;

        public RedisCacheService(IConnectionMultiplexer cache, ILogger<RedisCacheService> logger, IConfiguration config)
        {
            _logger = logger;
            _db = cache.GetDatabase();

            long? expiry = config.GetValue<long>("Cache:DefaultExpirySeconds");
            DefaultExpiry = TimeSpan.FromSeconds(expiry ?? 300);
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
                _logger.LogWarning("Failed to get key {key} from cache", key);
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
                _logger.LogWarning("Failed to set key {key} in cache", key);
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

        public async Task<bool> Exists(string key)
        {
            return await _db.KeyExistsAsync(key);
        }
    }
}
