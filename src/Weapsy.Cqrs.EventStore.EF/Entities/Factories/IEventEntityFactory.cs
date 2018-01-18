using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EventStore.EF.Entities.Factories
{
    public interface IEventEntityFactory
    {
        EventEntity CreateEvent(IDomainEvent @event, int version);
    }
}
