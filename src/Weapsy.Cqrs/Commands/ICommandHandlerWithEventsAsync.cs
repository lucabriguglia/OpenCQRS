using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Events;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerWithEventsAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
