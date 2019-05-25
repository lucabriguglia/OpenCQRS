using Kledex.Domain;

namespace Kledex.Store.EF.Entities.Factories
{
    public interface ICommandEntityFactory
    {
        CommandEntity CreateCommand(IDomainCommand command);
    }
}
