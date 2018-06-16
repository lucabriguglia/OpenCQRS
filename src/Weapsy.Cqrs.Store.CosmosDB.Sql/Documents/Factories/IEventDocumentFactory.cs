using OpenCqrs.Domain;

namespace OpenCqrs.Store.CosmosDB.Sql.Documents.Factories
{
    public interface IEventDocumentFactory
    {
        EventDocument CreateEvent(IDomainEvent @event, int version);
    }
}
