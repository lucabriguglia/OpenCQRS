using System;
using System.Threading.Tasks;

namespace Kledex.Caching.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, int cacheTime, object data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, int cacheTime, object data)
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
