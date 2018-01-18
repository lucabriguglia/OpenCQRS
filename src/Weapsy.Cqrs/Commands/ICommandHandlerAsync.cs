using System.Threading.Tasks;

namespace Weapsy.Cqrs.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
