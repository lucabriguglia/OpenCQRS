namespace OpenCqrs.Events
{
    /// <summary>
    /// IEventFactory
    /// </summary>
    public interface IEventFactory
    {
        /// <summary>
        /// Creates the concrete event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        dynamic CreateConcreteEvent(object @event);
    }
}
