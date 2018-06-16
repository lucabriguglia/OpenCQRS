using System.Collections.Generic;
using Weapsy.Cqrs.Events;

namespace Weapsy.Cqrs.Commands
{
    public interface ICommandHandlerWithEvents<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}