using System.Threading.Tasks;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Commands
{
    public interface ICommandHandlerWithAggregateAsync<in TCommand> where TCommand : IDomainCommand
    {
        Task<IAggregateRoot> HandleAsync(TCommand command);
    }
}
