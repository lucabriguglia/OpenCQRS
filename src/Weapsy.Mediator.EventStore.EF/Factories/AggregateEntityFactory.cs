using Weapsy.Mediator.Domain;
using Weapsy.Mediator.EventStore.EF.Entities;

namespace Weapsy.Mediator.EventStore.EF.Factories
{
    public class AggregateEntityFactory : IAggregateEntityFactory
    {
        public AggregateEntity CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            return new AggregateEntity
            {
                Id = @event.AggregateId,
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}