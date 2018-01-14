using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql.Documents.Factories
{
    public interface IEventDocumentFactory
    {
        EventDocument CreateEvent(IDomainEvent @event, int version);
    }
}
