using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface IDomainStore
    {
        /// <summary>
        /// Saves the event asynchronous.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="events">The events.</param>
        /// <returns></returns>
        Task SaveAsync<TAggregate>(IDomainCommand command, IList<IDomainEvent> events) 
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the events asynchronous.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId);

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="events">The events.</param>
        void Save<TAggregate>(IDomainCommand command, IList<IDomainEvent> events)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        IEnumerable<DomainEvent> GetEvents(Guid aggregateId);
    }

    public class DefaultDomainStore : IDomainStore
    {
        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public void Save<TAggregate>(IDomainCommand command, IList<IDomainEvent> events) 
            where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public Task SaveAsync<TAggregate>(IDomainCommand command, IList<IDomainEvent> events) 
            where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }
    }
}
