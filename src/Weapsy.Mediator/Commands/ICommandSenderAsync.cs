using System.Threading.Tasks;

namespace Weapsy.Mediator.Commands
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSenderAsync
    {
        /// <summary>
        /// Asynchronously sends the specified command.
        /// The command handler must implement ICommandHandlerAsync.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Asynchronously sends the command the and publish the events returned by the command handler.
        /// The command handler must implement ICommandHandlerWithEventsAsync.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        Task SendAndPublishAsync<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
