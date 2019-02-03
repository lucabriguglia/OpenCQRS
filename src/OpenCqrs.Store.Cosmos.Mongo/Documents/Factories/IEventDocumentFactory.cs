using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Store.Cosmos.Mongo.Documents.Factories
{
    public interface IEventDocumentFactory
    {
        EventDocument CreateEvent(IDomainEvent @event, long version);
    }
}
