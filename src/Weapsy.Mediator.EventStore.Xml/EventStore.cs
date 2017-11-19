using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.Xml
{
    public class EventStore : IEventStore
    {
        public Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}
