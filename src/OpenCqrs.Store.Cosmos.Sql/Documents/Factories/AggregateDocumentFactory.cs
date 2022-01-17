using System;

namespace Kledex.Store.Cosmos.Sql.Documents.Factories
{
    public class AggregateDocumentFactory : IAggregateDocumentFactory
    {
        public AggregateDocument CreateAggregate(Type aggregateType, Guid aggregateRootId)
        {
            return new AggregateDocument
            {
                Id = aggregateRootId,
                Type = aggregateType.AssemblyQualifiedName
            };
        }
    }
}