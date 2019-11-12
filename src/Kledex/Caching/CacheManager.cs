using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Options = Kledex.Configuration.Options;

namespace Kledex.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly Options _options;

        public CacheManager(ICacheProvider cacheProvider, IOptions<Options> options)
        {
            _cacheProvider = cacheProvider;
            _options = options.Value;
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> acquireAsync)
        {
            return GetOrCreateAsync(key, _options.CacheTime, acquireAsync);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, int cacheTime, Func<Task<T>> acquireAsync)
        {
            var data = await _cacheProvider.GetAsync<T>(key);

            if (data != null)
            {
                return data;
            }

            var result = await acquireAsync();

            await _cacheProvider.SetAsync(key, cacheTime, result);

            return result;
        }

        public Task RemoveAsync(string key)
        {
            return _cacheProvider.RemoveAsync(key);
        }

        public T GetOrCreate<T>(string key, Func<T> acquire)
        {
            return GetOrCreate(key, _options.CacheTime, acquire);
        }

        public T GetOrCreate<T>(string key, int cacheTime, Func<T> acquire)
        {
            var data = _cacheProvider.Get<T>(key);

            if (data != null)
            {
                return data;
            }

            var result = acquire();

            _cacheProvider.Set(key, cacheTime, result);

            return result;
        }

        public void Remove(string key)
        {
            _cacheProvider.Remove(key);
        }
    }
}
