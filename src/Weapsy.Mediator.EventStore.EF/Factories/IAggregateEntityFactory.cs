using Weapsy.Mediator.Domain;
using Weapsy.Mediator.EventStore.EF.Entities;

namespace Weapsy.Mediator.EventStore.EF.Factories
{
    public interface IAggregateEntityFactory
    {
        AggregateEntity CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
    }
}
