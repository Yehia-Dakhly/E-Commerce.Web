using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface ICacheRepository
    {
        // Get
        Task<string?> GetAsync(string Cachekey); 

        // Set
        Task SetAsync(string Cachekey, string value, TimeSpan TimeToLive);
    }
}
