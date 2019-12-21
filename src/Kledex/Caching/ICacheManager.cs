using System;
using System.Threading.Tasks;

namespace Kledex.Caching
{
    public interface ICacheManager
    {
        /// <summary>Gets or sets data asynchronously.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="acquireAsync">The acquire asynchronous.</param>
        /// <returns></returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquireAsync);

        /// <summary>Gets or sets data asynchronously.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="cacheTime"></param>
        /// <param name="acquireAsync">The acquire asynchronous.</param>
        /// <returns></returns>
        Task<T> GetOrSetAsync<T>(string key, int cacheTime, Func<Task<T>> acquireAsync);

        /// <summary>Removes data from cache asynchronously.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
