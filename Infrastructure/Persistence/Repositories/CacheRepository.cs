using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer _connection) : ICacheRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string?> GetAsync(string Cachekey)
        {
            var CacheValue = await _database.StringGetAsync(Cachekey);

            return CacheValue.IsNullOrEmpty ? null : CacheValue.ToString();
        }

        public async Task SetAsync(string Cachekey, string value, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(Cachekey, value, TimeToLive);
        }
    }
}
