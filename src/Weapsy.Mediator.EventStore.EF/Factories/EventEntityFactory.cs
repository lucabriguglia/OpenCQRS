using Newtonsoft.Json;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.EventStore.EF.Entities;

namespace Weapsy.Mediator.EventStore.EF.Factories
{
    public class EventEntityFactory : IEventEntityFactory
    {
        public EventEntity CreateEvent(IDomainEvent @event, int version)
        {
            return new EventEntity
            {
                AggregateId = @event.AggregateRootId,
                SequenceNumber = version,
                Type = @event.GetType().AssemblyQualifiedName,
                Body = JsonConvert.SerializeObject(@event),
                TimeStamp = @event.TimeStamp
            };
        }
    }
}