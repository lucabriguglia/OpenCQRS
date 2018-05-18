using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Commands
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command and publishes the events returned by the command handler.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithEvents&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command and the events returned by the handler will be published and saved to the event store.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithAggregate&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate: IAggregateRoot;
    }
}
