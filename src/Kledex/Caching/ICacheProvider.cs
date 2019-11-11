namespace Kledex.Caching
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Remove(string key);
    }
}
