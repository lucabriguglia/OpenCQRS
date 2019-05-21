using System.Threading.Tasks;

namespace OpenCqrs.Bus
{
    /// <summary>
    /// ITopicClient
    /// </summary>
    public interface ITopicClient
    {
        /// <summary>
        /// Sends the message asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage;
    }
}
