using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using OpenCqrs.Store.Cosmos.Sql.Configuration;
using OpenCqrs.Store.Cosmos.Sql.Documents;

namespace OpenCqrs.Store.Cosmos.Sql.Repositories
{
    internal class CommandRepository : BaseDocumentRepository<CommandDocument>
    {
        public CommandRepository(IDocumentClient documentClient, IOptions<DomainDbConfiguration> settings) 
            : base(settings.Value.CommandCollectionId, documentClient, settings)
        {
        }
    }
}