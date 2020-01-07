using System;

namespace Kledex.Commands
{
    [Obsolete("Please use ISagaCommandHandler instead.")]
    public interface ISequenceCommandHandler<in TCommand> where TCommand : ICommand
    {
        CommandResponse Handle(TCommand command, CommandResponse previousStepResponse);
    }
}
