using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Cqrs.Events;

namespace Weapsy.Cqrs.Commands
{
    public interface ICommandHandlerWithEventsAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
