using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface IDomainCommandHandlerAsync<in TCommand, TAggregate> where TCommand : IDomainCommand<TAggregate>
        where TAggregate : IAggregateRoot
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }
}
