using OpenCqrs.Domain;

namespace OpenCqrs.Commands
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandlerWithAggregate&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Sends the command and publishes the events returned by the command handler.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandlerWithEvents&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command and the events returned by the handler will be published and saved to the event store.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandlerWithAggregate&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate: IAggregateRoot;
    }
}
