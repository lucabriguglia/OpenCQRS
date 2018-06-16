using System.Threading.Tasks;

namespace Weapsy.Cqrs.Events
{
    /// <summary>
    /// IEventPublisherAsync
    /// </summary>
    public interface IEventPublisherAsync
    {
        /// <summary>
        /// Asynchronously publishes the specified event.
        /// The event handler must implement WeapsyCqrs.Events.IEventHandlerAsync.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : IEvent;
    }
}
