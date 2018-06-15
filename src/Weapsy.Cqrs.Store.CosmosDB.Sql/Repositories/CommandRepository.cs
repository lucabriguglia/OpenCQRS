using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Documents;

namespace Weapsy.Cqrs.Store.CosmosDB.Sql.Repositories
{
    internal class CommandRepository : BaseDocumentRepository<CommandDocument>
    {
        public CommandRepository(IDocumentClient documentClient, IOptions<StoreConfiguration> settings) 
            : base(settings.Value.CommandCollectionId, documentClient, settings)
        {
        }
    }
}