using Kledex.Commands;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Asynchronously sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement Kledex.Commands.ICommandHandlerWithWithDomainEventsAsync&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task<TResult> SendAsync<TResult>(IDomainCommand<IAggregateRoot, TResult> command);

        /// <summary>
        /// Sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement Kledex.Commands.ICommandHandlerWithDomainEvents&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
