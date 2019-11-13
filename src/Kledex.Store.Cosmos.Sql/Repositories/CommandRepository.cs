using Kledex.Store.Cosmos.Sql.Configuration;
using Kledex.Store.Cosmos.Sql.Documents;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;

namespace Kledex.Store.Cosmos.Sql.Repositories
{
    public class CommandRepository : BaseDocumentRepository<CommandDocument>
    {
        public CommandRepository(IDocumentClient documentClient, IOptions<DomainDbOptions> settings) 
            : base(settings.Value.CommandCollectionId, documentClient, settings)
        {
        }
    }
}