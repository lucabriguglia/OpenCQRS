using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql.Documents.Factories
{
    public class AggregateDocumentFactory : IAggregateDocumentFactory
    {
        public AggregateDocument CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            return new AggregateDocument
            {
                Id = @event.AggregateRootId,
                Type = typeof(TAggregate).AssemblyQualifiedName
            };
        }
    }
}