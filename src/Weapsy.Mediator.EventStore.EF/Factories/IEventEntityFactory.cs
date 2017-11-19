using Weapsy.Mediator.Domain;
using Weapsy.Mediator.EventStore.EF.Entities;

namespace Weapsy.Mediator.EventStore.EF.Factories
{
    public interface IEventEntityFactory
    {
        EventEntity CreateEvent(IDomainEvent @event, int version);
    }
}
