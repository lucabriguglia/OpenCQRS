using Kledex.Store.Cosmos.Sql.Documents;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;

namespace Kledex.Store.Cosmos.Sql.Repositories
{
    public class EventRepository : BaseDocumentRepository<EventDocument>
    {
        public EventRepository(IDocumentClient documentClient, IOptions<DomainDbOptions> settings) 
            : base(settings.Value.EventCollectionId, documentClient, settings)
        {
        }
    }
}