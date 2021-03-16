using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Infrastructure.Services
{
    public class CacheStorageService : ICacheStorageService
    {
        private readonly IDistributedCache _distributedCache;
        private const string  CachePrefix = "SFA.DAS.EmployerDemand";
        
        public CacheStorageService (IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        
        public async Task<T> RetrieveFromCache<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync($"{CachePrefix}_{key}");
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public async Task SaveToCache<T>(string key, T item, TimeSpan expiryTimeFromNow)
        {
            var json = JsonConvert.SerializeObject(item);

            await _distributedCache.SetStringAsync($"{CachePrefix}_{key}", json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiryTimeFromNow
            });
        }

        public async Task DeleteFromCache(string key)
        {
            await _distributedCache.RemoveAsync($"{CachePrefix}_{key}");
        }
    }
}