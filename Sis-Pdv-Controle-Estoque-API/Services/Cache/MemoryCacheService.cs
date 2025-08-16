using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_API.Services.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<MemoryCacheService> _logger;
        private readonly HashSet<string> _cacheKeys;
        private readonly object _lockObject = new();

        public MemoryCacheService(
            IMemoryCache memoryCache, 
            IOptions<CacheOptions> cacheOptions,
            ILogger<MemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
            _cacheKeys = new HashSet<string>();
        }

        public Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (_memoryCache.TryGetValue(key, out var cachedValue))
                {
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                    
                    if (cachedValue is string jsonString)
                    {
                        var deserializedValue = JsonSerializer.Deserialize<T>(jsonString);
                        return Task.FromResult(deserializedValue);
                    }
                    
                    return Task.FromResult(cachedValue as T);
                }

                _logger.LogDebug("Cache miss for key: {Key}", key);
                return Task.FromResult<T?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cache value for key: {Key}", key);
                return Task.FromResult<T?>(null);
            }
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
            {
                var cacheExpiration = expiration ?? TimeSpan.FromMinutes(_cacheOptions.DefaultExpirationMinutes);
                
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheExpiration,
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheOptions.SlidingExpirationMinutes),
                    Priority = CacheItemPriority.Normal
                };

                // Store as JSON string for complex objects to ensure proper serialization
                var jsonValue = JsonSerializer.Serialize(value);
                _memoryCache.Set(key, jsonValue, cacheEntryOptions);

                lock (_lockObject)
                {
                    _cacheKeys.Add(key);
                }

                _logger.LogDebug("Cache set for key: {Key} with expiration: {Expiration}", key, cacheExpiration);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache value for key: {Key}", key);
                return Task.CompletedTask;
            }
        }

        public Task RemoveAsync(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                
                lock (_lockObject)
                {
                    _cacheKeys.Remove(key);
                }

                _logger.LogDebug("Cache removed for key: {Key}", key);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache value for key: {Key}", key);
                return Task.CompletedTask;
            }
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var keysToRemove = new List<string>();

                lock (_lockObject)
                {
                    keysToRemove.AddRange(_cacheKeys.Where(key => regex.IsMatch(key)));
                }

                foreach (var key in keysToRemove)
                {
                    _memoryCache.Remove(key);
                    lock (_lockObject)
                    {
                        _cacheKeys.Remove(key);
                    }
                }

                _logger.LogDebug("Cache removed for pattern: {Pattern}, removed {Count} keys", pattern, keysToRemove.Count);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache values by pattern: {Pattern}", pattern);
                return Task.CompletedTask;
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? expiration = null) where T : class
        {
            var cachedValue = await GetAsync<T>(key);
            if (cachedValue != null)
            {
                return cachedValue;
            }

            var item = await getItem();
            if (item != null)
            {
                await SetAsync(key, item, expiration);
            }

            return item;
        }
    }
}