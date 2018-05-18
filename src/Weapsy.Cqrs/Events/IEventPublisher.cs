namespace Weapsy.Cqrs.Events
{
    /// <summary>
    /// IEventPublisher
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the specified event.
        /// The event handler must implement Weapsy.Cqrs.Events.IEventHandler.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        void Publish<TEvent>(TEvent @event) 
            where TEvent : IEvent;
    }
}
