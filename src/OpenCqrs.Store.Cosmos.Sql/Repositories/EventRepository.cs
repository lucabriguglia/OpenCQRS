using Kledex.Store.Cosmos.Sql.Configuration;
using Kledex.Store.Cosmos.Sql.Documents;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;

namespace Kledex.Store.Cosmos.Sql.Repositories
{
    public class EventRepository : BaseDocumentRepository<EventDocument>
    {
        public EventRepository(IDocumentClient documentClient, IOptions<CosmosDbOptions> settings) 
            : base(settings.Value.EventCollectionId, documentClient, settings)
        {
        }
    }
}