using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.EF.Entities.Factories
{
    public interface ICommandEntityFactory
    {
        CommandEntity CreateCommand(IDomainCommand command);
    }
}
