using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenCqrs.Domain
{
    public interface IDomainCommandHandlerAsync<in TCommand> where TCommand : IDomainCommand
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }
}
