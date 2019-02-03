using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Store.Cosmos.Mongo.Documents.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(IDomainCommand command);
    }
}
