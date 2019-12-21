using System;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        /// <summary>
        /// Saves the specified aggregate asynchronous.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <returns></returns>
        Task SaveAsync(T aggregate);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(Guid id);   
    }
}