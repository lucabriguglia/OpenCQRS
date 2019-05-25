using System;
using Kledex.Domain;

namespace Kledex.Store.Cosmos.Sql.Documents.Factories
{
    public class AggregateDocumentFactory : IAggregateDocumentFactory
    {
        public AggregateDocument CreateAggregate<TAggregate>(Guid aggregateRootId) where TAggregate : IAggregateRoot
        {
            return new AggregateDocument
            {
                Id = aggregateRootId,
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}