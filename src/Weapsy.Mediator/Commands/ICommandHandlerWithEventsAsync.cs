using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Commands
{
    public interface ICommandHandlerWithEventsAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
