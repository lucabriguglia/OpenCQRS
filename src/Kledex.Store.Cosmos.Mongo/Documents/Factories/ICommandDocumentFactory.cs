using Kledex.Domain;

namespace Kledex.Store.Cosmos.Mongo.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
