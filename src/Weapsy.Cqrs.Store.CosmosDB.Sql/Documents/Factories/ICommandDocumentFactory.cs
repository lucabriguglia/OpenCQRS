using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.CosmosDB.Sql.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
