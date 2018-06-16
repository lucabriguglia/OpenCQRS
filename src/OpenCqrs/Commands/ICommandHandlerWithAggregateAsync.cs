using System.Threading.Tasks;
using OpenCqrs.Domain;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerWithAggregateAsync<in TCommand> where TCommand : IDomainCommand
    {
        Task<IAggregateRoot> HandleAsync(TCommand command);
    }
}
