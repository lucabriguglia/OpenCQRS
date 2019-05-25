using System;
using Kledex.Domain;

namespace Kledex.Store.EF.Entities.Factories
{
    public class AggregateEntityFactory : IAggregateEntityFactory
    {
        public AggregateEntity CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot
        {
            return new AggregateEntity
            {
                Id = aggregateRootId,
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}