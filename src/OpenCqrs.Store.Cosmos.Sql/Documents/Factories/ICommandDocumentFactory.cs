using Kledex.Domain;

namespace Kledex.Store.Cosmos.Sql.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
