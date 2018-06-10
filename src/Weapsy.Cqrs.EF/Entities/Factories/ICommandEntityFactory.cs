using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.EF.Entities.Factories
{
    public interface ICommandEntityFactory
    {
        CommandEntity CreateCommand(IDomainCommand command);
    }
}
