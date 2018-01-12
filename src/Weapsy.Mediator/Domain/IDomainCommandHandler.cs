namespace Weapsy.Mediator.Domain
{
    public interface IDomainCommandHandler<in TCommand> where TCommand : IDomainCommand
    {
        IAggregateRoot Handle(TCommand command);
    }
}
