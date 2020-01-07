using System.Threading.Tasks;

namespace Kledex.Commands
{
    public interface ISagaCommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<CommandResponse> HandleAsync(TCommand command, CommandResponse previousStepResponse);
    }
}
