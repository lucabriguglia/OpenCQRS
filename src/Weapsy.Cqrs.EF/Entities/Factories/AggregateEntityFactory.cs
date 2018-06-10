using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EF.Entities.Factories
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