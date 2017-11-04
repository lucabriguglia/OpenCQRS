using System.Threading.Tasks;

namespace Weapsy.Mediator.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
