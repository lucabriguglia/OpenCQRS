using OpenCqrs.Domain;

namespace OpenCqrs.Store.CosmosDB.Sql.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
