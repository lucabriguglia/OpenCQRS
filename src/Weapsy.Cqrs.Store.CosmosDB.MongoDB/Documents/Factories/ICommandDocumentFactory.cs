using OpenCqrs.Domain;

namespace OpenCqrs.Store.CosmosDB.MongoDB.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
