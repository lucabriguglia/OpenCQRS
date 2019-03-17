using System.Collections.Generic;
using OpenCqrs.Events;

namespace OpenCqrs.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}