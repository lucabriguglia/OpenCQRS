using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Events;

namespace Kledex.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}
