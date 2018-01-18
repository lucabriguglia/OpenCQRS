using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Documents.Factories
{
    public class AggregateDocumentFactory : IAggregateDocumentFactory
    {
        public AggregateDocument CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            return new AggregateDocument
            {
                Id = @event.AggregateRootId.ToString(),
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}