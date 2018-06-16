using System;
using OpenCqrs.Domain;

namespace OpenCqrs.Store.EF.Entities.Factories
{
    public interface IAggregateEntityFactory
    {
        AggregateEntity CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot;
    }
}
