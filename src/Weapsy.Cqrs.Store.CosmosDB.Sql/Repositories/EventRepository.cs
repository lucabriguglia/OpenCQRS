using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Documents;

namespace Weapsy.Cqrs.Store.CosmosDB.Sql.Repositories
{
    internal class EventRepository : BaseDocumentRepository<EventDocument>
    {
        public EventRepository(IDocumentClient documentClient, IOptions<StoreConfiguration> settings) 
            : base(settings.Value.EventCollectionId, documentClient, settings)
        {
        }
    }
}