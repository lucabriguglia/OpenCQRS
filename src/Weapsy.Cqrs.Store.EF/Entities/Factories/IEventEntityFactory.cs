using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.EF.Entities.Factories
{
    public interface IEventEntityFactory
    {
        EventEntity CreateEvent(IDomainEvent @event, int version);
    }
}
