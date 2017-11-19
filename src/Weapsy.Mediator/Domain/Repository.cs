using System;
using System.Threading.Tasks;

namespace Weapsy.Mediator.Domain
{
    /// <inheritdoc />
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Weapsy.Mediator.Domain.IRepository{T}" />
    public class Repository<T> : IRepository<T> where T : IAggregateRoot
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task SaveAsync(T aggregate)
        {
            foreach (var @event in aggregate.Events)
                await _eventStore.SaveEventAsync<T>(@event);
        }

        public void Save(T aggregate)
        {
            foreach (var @event in aggregate.Events)
                _eventStore.SaveEvent<T>(@event);       
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var aggregate = Activator.CreateInstance<T>(); // looking for a better solution without using new()
            var events = await _eventStore.GetEventsAsync(id);
            aggregate.ApplyEvents(events);
            return aggregate;
        }

        public T GetById(Guid id)
        {
            var aggregate = Activator.CreateInstance<T>(); // looking for a better solution without using new()
            var events = _eventStore.GetEvents(id);
            aggregate.ApplyEvents(events);
            return aggregate;
        }
    }
}