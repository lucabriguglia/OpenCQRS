using System.Collections.Generic;

namespace Weapsy.Mediator.Domain
{
    public interface IDomainCommandHandler<in TCommand> where TCommand : IDomainCommand
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}
