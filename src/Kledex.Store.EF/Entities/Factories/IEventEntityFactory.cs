using Kledex.Domain;

namespace Kledex.Store.EF.Entities.Factories
{
    public interface IEventEntityFactory
    {
        EventEntity CreateEvent(IDomainEvent @event, int version);
    }
}
