namespace Weapsy.Cqrs.Domain
{
    public interface IDomainCommandHandler<in TCommand> where TCommand : IDomainCommand
    {
        IAggregateRoot Handle(TCommand command);
    }
}
