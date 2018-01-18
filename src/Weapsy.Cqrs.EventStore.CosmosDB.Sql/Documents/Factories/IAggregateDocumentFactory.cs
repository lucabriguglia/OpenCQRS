using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EventStore.CosmosDB.Sql.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
    }
}
