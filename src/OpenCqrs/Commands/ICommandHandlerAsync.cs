using System.Threading.Tasks;
using OpenCqrs.Abstractions.Commands;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
