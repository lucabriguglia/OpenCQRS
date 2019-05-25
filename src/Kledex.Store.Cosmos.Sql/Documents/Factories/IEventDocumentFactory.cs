using Kledex.Domain;

namespace Kledex.Store.Cosmos.Sql.Documents.Factories
{
    public interface IEventDocumentFactory
    {
        EventDocument CreateEvent(IDomainEvent @event, int version);
    }
}
