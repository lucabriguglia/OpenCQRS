using System.Collections.Generic;
using Kledex.Events;

namespace Kledex.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}