using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Abstractions.Commands
{
    public interface ICommandHandlerWithDomainEventsAsync<in TCommand> where TCommand : IDomainCommand
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }
}
