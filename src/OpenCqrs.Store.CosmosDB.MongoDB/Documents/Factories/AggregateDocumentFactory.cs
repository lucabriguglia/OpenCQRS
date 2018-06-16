using System;
using OpenCqrs.Domain;

namespace OpenCqrs.Store.CosmosDB.MongoDB.Documents.Factories
{
    public class AggregateDocumentFactory : IAggregateDocumentFactory
    {
        public AggregateDocument CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot
        {
            return new AggregateDocument
            {
                Id = aggregateRootId.ToString(),
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}