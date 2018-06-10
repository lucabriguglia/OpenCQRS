using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Cqrs.Domain
{
    public interface IEventStore
    {
        /// <summary>
        /// Saves the event asynchronous.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        Task SaveEventAsync<TAggregate>(IDomainEvent @event) 
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="event">The event.</param>
        void SaveEvent<TAggregate>(IDomainEvent @event) 
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the events asynchronous.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId);

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        IEnumerable<DomainEvent> GetEvents(Guid aggregateId);

        /// <summary>
        /// Gets the events by command identifier asynchronously.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<DomainEvent>> GetEventsByCommandIdAsync(Guid aggregateId, Guid commandId);

        /// <summary>
        /// Gets the events by command identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        /// <returns></returns>
        IEnumerable<DomainEvent> GetEventsByCommandId(Guid aggregateId, Guid commandId);

        /// <summary>
        /// Gets the events by event types asynchronously.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        Task<IEnumerable<DomainEvent>> GetEventsByEventTypesAsync(Guid aggregateId, IEnumerable<Type> types);

        /// <summary>
        /// Gets the events by event types.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        IEnumerable<DomainEvent> GetEventsByEventTypes(Guid aggregateId, IEnumerable<Type> types);
    }
}
