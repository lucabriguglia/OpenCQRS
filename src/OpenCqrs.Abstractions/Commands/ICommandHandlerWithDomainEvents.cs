using System.Collections.Generic;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Abstractions.Commands
{
    public interface ICommandHandlerWithDomainEvents<in TCommand> where TCommand : IDomainCommand
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}