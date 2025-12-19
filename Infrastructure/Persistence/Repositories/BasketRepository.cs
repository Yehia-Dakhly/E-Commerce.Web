using DomainLayer.Models.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket customerBasket, TimeSpan? timeSpan = null)
        {
            var JsonBasket = JsonSerializer.Serialize(customerBasket);
            var iscreatedorupdated = await _database.StringSetAsync(customerBasket.Id, JsonBasket, timeSpan ?? TimeSpan.FromDays(30));
            if (iscreatedorupdated)
            {
                return await GetBasketAsync(customerBasket.Id);
            }
            return null;
        }

        public async Task<bool> DeleteBasketAsync(string Key)
        => await _database.KeyDeleteAsync(Key);

        public async Task<CustomerBasket?> GetBasketAsync(string Key)
        {
            var Basket = await _database.StringGetAsync(Key);
            if (Basket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
        }
    }
}
