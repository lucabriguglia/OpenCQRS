using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.CosmosDB.MongoDB.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
