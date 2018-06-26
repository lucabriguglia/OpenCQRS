using OpenCqrs.Domain;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerWithAggregate<in TCommand> where TCommand : IDomainCommand
    {
        IAggregateRoot Handle(TCommand command);
    }
}
