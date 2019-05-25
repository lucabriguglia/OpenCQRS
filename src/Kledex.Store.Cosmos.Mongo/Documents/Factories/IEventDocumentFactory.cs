using Kledex.Domain;

namespace Kledex.Store.Cosmos.Mongo.Documents.Factories
{
    public interface IEventDocumentFactory
    {
        EventDocument CreateEvent(IDomainEvent @event, long version);
    }
}
