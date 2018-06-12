using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.CosmosDB.MongoDB.Documents.Factories
{
    public interface IAggregateDocumentFactory
    {
        AggregateDocument CreateAggregate<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
    }
}
