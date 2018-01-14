using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
    }
}
