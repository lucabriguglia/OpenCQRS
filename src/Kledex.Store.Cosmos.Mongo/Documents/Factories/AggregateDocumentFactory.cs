using System;
using Kledex.Domain;

namespace Kledex.Store.Cosmos.Mongo.Documents.Factories
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