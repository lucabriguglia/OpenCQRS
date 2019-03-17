using System.Collections.Generic;

namespace OpenCqrs.Domain
{
    public interface IDomainCommandHandler<in TCommand> where TCommand : IDomainCommand
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}