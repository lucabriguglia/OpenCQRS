using System.Threading.Tasks;

namespace Kledex.Domain
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface IDomainCommandSender
    {
        /// <summary>
        /// Asynchronously sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement Kledex.Commands.ICommandHandlerWithWithDomainEventsAsync&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SendAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement Kledex.Commands.ICommandHandlerWithDomainEvents&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;
    }
}
