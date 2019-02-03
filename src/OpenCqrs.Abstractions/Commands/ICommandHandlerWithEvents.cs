using System.Collections.Generic;
using OpenCqrs.Abstractions.Events;

namespace OpenCqrs.Abstractions.Commands
{
    public interface ICommandHandlerWithEvents<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}