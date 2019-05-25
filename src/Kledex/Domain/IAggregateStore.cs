using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface IAggregateStore
    {
        /// <summary>
        /// Saves the aggregate asynchronously.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task SaveAggregateAsync<TAggregate>(Guid id)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Saves the aggregate.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="id">The identifier.</param>
        void SaveAggregate<TAggregate>(Guid id)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the aggregates asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AggregateStoreModel>> GetAggregatesAsync();

        /// <summary>
        /// Gets the aggregates.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AggregateStoreModel> GetAggregates();
    }
}
