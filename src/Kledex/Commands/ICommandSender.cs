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
        /// Sends the specified command asynchronously.
        /// The command handler must implement Kledex.Commands.ICommandHandlerAsync&lt;TCommand&gt;.
        /// </summary>
        /// <param name="command">The command.</param>
        Task SendAsync(ICommand command);

        /// <summary>
        /// Sends the specified commands asynchronously.
        /// The command handler must implement Kledex.Commands.ISequenceCommandHandlerAsync&lt;TCommand&gt;.
        /// </summary>
        /// <param name="command">The command.</param>
        Task SendAsync(params ICommand[] commands);

        /// <summary>
        /// Sends the specified command asynchronously.
        /// The command handler must implement Kledex.Commands.ICommandHandlerAsync&lt;TCommand&gt;.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A custom object set as result in the command hadler response.</returns>
        Task<TResult> SendAsync<TResult>(ICommand command);

        /// <summary>
        /// Sends the specified commands asynchronously.
        /// The command handler must implement Kledex.Commands.ISequenceCommandHandlerAsync&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="commands">The commands.</param>
        /// <returns>A custom object set as result in the command hadler response.</returns>
        Task<TResult> SendAsync<TResult>(params ICommand[] commands);

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement Kledex.Commands.ICommandHandler&lt;TCommand&gt;.
        /// </summary>
        /// <param name="command">The command.</param>
        void Send(ICommand command);

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement Kledex.Commands.ICommandHandler&lt;TCommand&gt;.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A custom object set as result in the command hadler response.</returns>
        TResult Send<TResult>(ICommand command);
    }
}
