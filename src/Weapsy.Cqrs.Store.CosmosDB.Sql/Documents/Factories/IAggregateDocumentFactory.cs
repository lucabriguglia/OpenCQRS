using System;
using OpenCqrs.Domain;

namespace OpenCqrs.Store.CosmosDB.Sql.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot;
    }
}
