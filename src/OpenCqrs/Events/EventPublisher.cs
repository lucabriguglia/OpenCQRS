using System;
using OpenCqrs.Bus;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Events
{
    /// <inheritdoc />
    /// <summary>
    /// EventPublisher
    /// </summary>
    /// <seealso cref="T:OpenCqrs.Events.IEventPublisher" />
    public class EventPublisher : IEventPublisher
    {
        private readonly IResolver _resolver;
        private readonly IBusMessageDispatcher _busMessageDispatcher;

        public EventPublisher(IResolver resolver, IBusMessageDispatcher busMessageDispatcher)
        {
            _resolver = resolver;
            _busMessageDispatcher = busMessageDispatcher;
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var handlers = _resolver.ResolveAll<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
                handler.Handle(@event);

            if (@event is IBusMessage message)
                _busMessageDispatcher.DispatchAsync(message).GetAwaiter().GetResult();
        }
    }
}
