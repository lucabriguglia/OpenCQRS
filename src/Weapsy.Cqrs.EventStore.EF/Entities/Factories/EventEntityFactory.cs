using Newtonsoft.Json;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EventStore.EF.Entities.Factories
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
                Data = JsonConvert.SerializeObject(@event),
                TimeStamp = @event.TimeStamp,
                UserId = @event.UserId,
                Source = @event.Source
            };
        }
    }
}