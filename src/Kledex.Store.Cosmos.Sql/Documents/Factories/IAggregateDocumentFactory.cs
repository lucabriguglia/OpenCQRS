using System;

namespace Kledex.Store.Cosmos.Sql.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate(Type aggregateType, Guid aggregateRootId);
    }
}
