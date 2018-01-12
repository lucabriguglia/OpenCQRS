using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.EF.Entities.Factories
{
    public interface IEventEntityFactory
    {
        EventEntity CreateEvent(IDomainEvent @event, int version);
    }
}
