using System.Collections.Generic;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Commands
{
    public interface ICommandHandlerWithEvents<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}