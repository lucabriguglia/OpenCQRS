using System;

namespace Kledex.Store.Cosmos.Mongo.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate(Type aggregateType, Guid aggregateRootId);
    }
}
