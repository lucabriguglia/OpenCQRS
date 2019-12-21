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

        /// <inheritdoc />
        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquireAsync)
        {
            return GetOrSetAsync(key, _options.CacheTime, acquireAsync);
        }

        /// <inheritdoc />
        public async Task<T> GetOrSetAsync<T>(string key, int cacheTime, Func<Task<T>> acquireAsync)
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

        /// <inheritdoc />
        public Task RemoveAsync(string key)
        {
            return _cacheProvider.RemoveAsync(key);
        }
    }
}
