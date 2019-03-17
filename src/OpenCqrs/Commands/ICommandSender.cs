using System.Threading.Tasks;
using OpenCqrs.Domain;

namespace OpenCqrs.Commands
{
    /// <summary>
    /// ICommandSender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Asynchronously sends the specified command.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandlerAsync&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandler&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
