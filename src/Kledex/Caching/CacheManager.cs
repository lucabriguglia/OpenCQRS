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

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> acquire)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOrCreateAsync<T>(string key, int cacheTime, Func<Task<T>> acquire)
        {
            throw new NotImplementedException();
        }

        public T GetOrCreate<T>(string key, Func<T> acquire)
        {
            return GetOrCreate(key, _options.CacheTime, acquire);
        }

        public T GetOrCreate<T>(string key, int cacheTime, Func<T> acquire)
        {
            var data = _cacheProvider.Get<T>(key);

            if (data != null)
                return data;

            var result = acquire();

            _cacheProvider.Set(key, result, cacheTime);

            return result;
        }
    }
}
