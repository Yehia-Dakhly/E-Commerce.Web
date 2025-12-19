using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public Task<string?> GetAsync(string cacheKey)
        => cacheRepository.GetAsync(cacheKey);

        public async Task SetAsync(string CacheKey, object CahceValue, TimeSpan TimeToLive)
        {
            var Value = JsonSerializer.Serialize(CahceValue);
            await cacheRepository.SetAsync(CacheKey, Value, TimeToLive); 
        }
    }
}
