using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Commands
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement ICommandHandler&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command the and publish the events returned by the command handler.
        /// The command handler must implement ICommandHandlerWithEvents&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command the and publish the events returned by the command handler.
        /// The command handler must implement ICommandHandlerWithEvents&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate: IAggregateRoot;
    }
}
