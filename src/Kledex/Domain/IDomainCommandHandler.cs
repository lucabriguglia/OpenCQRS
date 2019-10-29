using System.Collections.Generic;

namespace Kledex.Domain
{
    public interface IDomainCommandHandler<in TCommand>
        where TCommand : IDomainCommand<IAggregateRoot>
    {
        IEnumerable<IDomainEvent> Handle(TCommand command);
    }
}