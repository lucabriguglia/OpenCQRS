using System.Collections.Generic;
using OpenCqrs.Domain;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerWithDomainEvents<in TCommand> where TCommand : IDomainCommand
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}