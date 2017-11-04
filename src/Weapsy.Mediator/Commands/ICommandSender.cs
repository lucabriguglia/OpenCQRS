namespace Weapsy.Mediator.Commands
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement Weapsy.Mediator.Commands.ICommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command the and publish the events returned by the command handler.
        /// The command handler must implement ICommandHandlerWithEvents.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
