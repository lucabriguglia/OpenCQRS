using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    /// <inheritdoc />
    public class Repository<T> : IRepository<T> where T : IAggregateRoot
    {
        private readonly IDomainStore _domainStore;

        public Repository(IDomainStore domainStore)
        {
            _domainStore = domainStore;
        }

        /// <inheritdoc />
        public async Task SaveAsync(T aggregate)
        {
            await _domainStore.SaveAsync<T>(aggregate.Id, null, aggregate.Events);
        }

        /// <inheritdoc />
        public void Save(T aggregate)
        {
            _domainStore.Save<T>(aggregate.Id, null, aggregate.Events);       
        }

        /// <inheritdoc />
        public async Task<T> GetByIdAsync(Guid id)
        {
            var events = await _domainStore.GetEventsAsync(id);
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
            var events = _domainStore.GetEvents(id);
            var domainEvents = events as DomainEvent[] ?? events.ToArray();
            if (!domainEvents.Any())
                return default(T);

            var aggregate = Activator.CreateInstance<T>();          
            aggregate.LoadsFromHistory(domainEvents);
            return aggregate;
        }
    }
}