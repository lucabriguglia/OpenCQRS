using System.Threading.Tasks;

namespace Weapsy.Cqrs.Domain
{
    public interface IDomainCommandHandlerAsync<in TCommand> where TCommand : IDomainCommand
    {
        Task<IAggregateRoot> HandleAsync(TCommand command);
    }
}
