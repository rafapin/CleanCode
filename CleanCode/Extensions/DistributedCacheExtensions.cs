using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanCode.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string RecordId, 
            T data,
            TimeSpan? absoluteSpireTime = null,
            TimeSpan? unuseExpiredTime=null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteSpireTime ?? TimeSpan.FromSeconds(1);
            options.SlidingExpiration = unuseExpiredTime;
            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(RecordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string RecordId)
        {
            var jsonData = await cache.GetStringAsync(RecordId);
            if(jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }


    }
}
