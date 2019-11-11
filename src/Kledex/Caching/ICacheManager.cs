using System;
using System.Threading.Tasks;

namespace Kledex.Caching
{
    public interface ICacheManager
    {
        Task<T> GetOrCreateAsync<T>(string key, int cacheTime, Func<Task<T>> acquire);
        Task<bool> IsSetAsync(string key);
        Task<bool> RemoveAsync(string key);
        T GetOrCreate<T>(string key, int cacheTime, Func<T> acquire);        
        bool IsSet(string key);
        void Remove(string key);
    }
}
