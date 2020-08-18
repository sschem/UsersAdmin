using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Services;

namespace Tatisoft.UsersAdmin.Services
{
    public class AppCache : IAppCache
    {
        private readonly IDistributedCache _cache;

        public AppCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync<T>(string key, T t)
        {
            var serializedObject = JsonConvert.SerializeObject(t);
            var encodedObject = Encoding.UTF8.GetBytes(serializedObject);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(new TimeSpan(0, 1, 0));
            await _cache.SetAsync(key, encodedObject, options);
        }

        public async Task<T> GetAsync<T>(string key) where T: class
        {
            T res = null;
            var obtainedEncoded = await _cache.GetAsync(key);
            if (obtainedEncoded != null)
            {
                var obtainedJson = Encoding.UTF8.GetString(obtainedEncoded);
                res = JsonConvert.DeserializeObject<T>(obtainedJson);
            }
            return res;
        }

        public Task RemoveAsync(string key)
        {
            return _cache.RemoveAsync(key);
        }
    }
}