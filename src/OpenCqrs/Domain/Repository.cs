using System;
using System.Linq;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Domain
{
    /// <inheritdoc />
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IRepository{T}" />
    public class Repository<T> : IRepository<T> where T : IAggregateRoot
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        /// <inheritdoc />
        public async Task SaveAsync(T aggregate)
        {
            foreach (var @event in aggregate.Events)
                await _eventStore.SaveEventAsync<T>(@event, null);
        }

        /// <inheritdoc />
        public void Save(T aggregate)
        {
            foreach (var @event in aggregate.Events)
                _eventStore.SaveEvent<T>(@event, null);       
        }

        /// <inheritdoc />
        public async Task<T> GetByIdAsync(Guid id)
        {
            var events = await _eventStore.GetEventsAsync(id);
            var domainEvents = events as DomainEvent[] ?? events.ToArray();
            if (!domainEvents.Any())
                return default(T);

            var aggregate = Activator.CreateInstance<T>();        
            aggregate.LoadsFromHistory(domainEvents);
            return aggregate;
        }

        /// <inheritdoc />
        public T GetById(Guid id)
        {
            var events = _eventStore.GetEvents(id);
            var domainEvents = events as DomainEvent[] ?? events.ToArray();
            if (!domainEvents.Any())
                return default(T);

            var aggregate = Activator.CreateInstance<T>();          
            aggregate.LoadsFromHistory(domainEvents);
            return aggregate;
        }
    }
}