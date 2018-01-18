using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EventStore.EF.Entities.Factories
{
    public interface IAggregateEntityFactory
    {
        AggregateEntity CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
    }
}
