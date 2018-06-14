using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.CosmosDB.MongoDB.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot;
    }
}
