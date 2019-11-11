using System;
using System.Threading.Tasks;

namespace Kledex.Caching
{
    public static class CacheProviderExtensions
    {
        public static T Get<T>(this ICacheProvider cacheProvider, string key, Func<T> acquire)
        {
            return Get(cacheProvider, key, 60, acquire);
        }

        public static T Get<T>(this ICacheProvider cacheProvider, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheTime <= 0)
                return acquire();

            var data = cacheProvider.Get<T>(key);

            if (data != null)
                return data;

            var result = acquire();

            cacheProvider.Set(key, result, cacheTime);

            return result;
        }

        public static async Task<T> GetAsync<T>(this ICacheProvider cacheProvider, string key, Func<Task<T>> acquire)
        {
            return await GetAsync(cacheProvider, key, 60, acquire);
        }

        public static Task<T> GetAsync<T>(this ICacheProvider cacheProvider, string key, int cacheTime, Func<Task<T>> acquire)
        {
            //if (cacheTime <= 0)
            //    return await acquire();

            //var data = await cacheProvider.GetOrCreateAsync(key, cacheTime, acquire);

            //return data;

            throw new NotImplementedException();
        }
    }
}
