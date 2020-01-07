namespace Kledex.Commands
{
    public interface ISagaCommandHandler<in TCommand> where TCommand : ICommand
    {
        CommandResponse Handle(TCommand command, CommandResponse previousStepResponse);
    }
}
