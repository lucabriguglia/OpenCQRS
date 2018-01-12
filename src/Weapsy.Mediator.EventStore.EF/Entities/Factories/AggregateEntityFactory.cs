using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.EF.Entities.Factories
{
    public class AggregateEntityFactory : IAggregateEntityFactory
    {
        public AggregateEntity CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            return new AggregateEntity
            {
                Id = @event.AggregateRootId,
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}