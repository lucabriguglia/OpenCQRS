using System;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Store.Cosmos.Mongo.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot;
    }
}
