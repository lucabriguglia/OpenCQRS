using System.Threading.Tasks;

namespace Kledex.Events
{
    /// <summary>
    /// IEventPublisher
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Asynchronously publishes the specified event.
        /// The event handler must implement Kledex.Events.IEventHandlerAsync&lt;TEvent&gt;.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;

        /// <summary>
        /// Publishes the specified event.
        /// The event handler must implement Kledex.Events.IEventHandler&lt;TEvent&gt;.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        void Publish<TEvent>(TEvent @event) 
            where TEvent : IEvent;
    }
}
