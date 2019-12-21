using Kledex.Store.Cosmos.Sql.Documents;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;

namespace Kledex.Store.Cosmos.Sql.Repositories
{
    public class AggregateRepository : BaseDocumentRepository<AggregateDocument>
    {
        public AggregateRepository(IDocumentClient documentClient, IOptions<DomainDbOptions> settings) 
            : base(settings.Value.AggregateCollectionId, documentClient, settings)
        {
        }
    }
}