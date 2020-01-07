using System;
using System.Threading.Tasks;

namespace Kledex.Commands
{
    [Obsolete("Please use ISagaCommandHandlerAsync instead.")]
    public interface ISequenceCommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<CommandResponse> HandleAsync(TCommand command, CommandResponse previousStepResponse);
    }
}
