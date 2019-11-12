using System.Threading.Tasks;

namespace Kledex.Caching
{
    public interface ICacheProvider
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, int cacheTime, object data);
        Task<bool> IsSetAsync(string key);
        Task RemoveAsync(string key);
        T Get<T>(string key);
        void Set(string key, int cacheTime, object data);
        bool IsSet(string key);
        void Remove(string key);
    }
}
