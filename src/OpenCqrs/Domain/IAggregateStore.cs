using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenCqrs.Domain
{
    public interface IAggregateStore
    {
        /// <summary>
        /// Saves the aggregate asynchronously.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregate">The aggregate.</param>
        /// <returns></returns>
        Task SaveAggregateAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the aggregates asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AggregateRoot>> GetAggregatesAsync();

        /// <summary>
        /// Saves the aggregate.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregate">The aggregate.</param>
        void SaveAggregate<TAggregate>(TAggregate aggregate)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the aggregates.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AggregateRoot> GetAggregates();
    }
}
