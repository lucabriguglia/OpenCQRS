using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Events;

namespace OpenCqrs.Abstractions.Commands
{
    public interface ICommandHandlerWithEventsAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
