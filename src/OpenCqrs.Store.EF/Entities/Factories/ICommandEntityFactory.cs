using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Store.EF.Entities.Factories
{
    public interface ICommandEntityFactory
    {
        CommandEntity CreateCommand(IDomainCommand command);
    }
}
