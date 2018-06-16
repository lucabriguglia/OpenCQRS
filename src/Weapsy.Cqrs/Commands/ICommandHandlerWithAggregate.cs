using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Commands
{
    public interface ICommandHandlerWithAggregate<in TCommand> where TCommand : IDomainCommand
    {
        IAggregateRoot Handle(TCommand command);
    }
}
