using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Events;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
