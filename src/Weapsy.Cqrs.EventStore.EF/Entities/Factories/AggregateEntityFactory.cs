using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EventStore.EF.Entities.Factories
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