using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using OpenCqrs.Store.Cosmos.Sql.Configuration;
using OpenCqrs.Store.Cosmos.Sql.Documents;

namespace OpenCqrs.Store.Cosmos.Sql.Repositories
{
    internal class EventRepository : BaseDocumentRepository<EventDocument>
    {
        public EventRepository(IDocumentClient documentClient, IOptions<DomainDbConfiguration> settings) 
            : base(settings.Value.EventCollectionId, documentClient, settings)
        {
        }
    }
}