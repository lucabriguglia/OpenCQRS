using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface IDomainCommandHandlerAsync<in TCommand> 
        where TCommand : IDomainCommand<IAggregateRoot>
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }
}
