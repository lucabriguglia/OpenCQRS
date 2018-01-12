using Newtonsoft.Json;
using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.EF.Entities.Factories
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