using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.EF.Entities.Factories
{
    public interface IAggregateEntityFactory
    {
        AggregateEntity CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
    }
}
