using System.Threading.Tasks;

namespace Kledex.Bus
{
    /// <summary>
    /// IQueueClient
    /// </summary>
    public interface IQueueClient
    {
        /// <summary>
        /// Sends the message asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage;
    }
}
