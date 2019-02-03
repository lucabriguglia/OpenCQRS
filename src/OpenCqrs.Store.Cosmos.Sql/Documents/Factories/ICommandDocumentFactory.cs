using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Store.Cosmos.Sql.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
