using System;
using Kledex.Domain;

namespace Kledex.Store.EF.Entities.Factories
{
    public interface IAggregateEntityFactory
    {
        AggregateEntity CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot;
    }
}
