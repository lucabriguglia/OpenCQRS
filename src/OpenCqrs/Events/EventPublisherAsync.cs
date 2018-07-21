using System;
using System.Threading.Tasks;
using OpenCqrs.Bus;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Events
{
    /// <inheritdoc />
    /// <summary>
    /// EventPublisherAsync
    /// </summary>
    /// <seealso cref="T:OpenCqrs.Events.IEventPublisherAsync" />
    public class EventPublisherAsync : IEventPublisherAsync
    {
        private readonly IResolver _resolver;
        private readonly IBusMessageDispatcher _busMessageDispatcher;

        public EventPublisherAsync(IResolver resolver, IBusMessageDispatcher busMessageDispatcher)
        {
            _resolver = resolver;
            _busMessageDispatcher = busMessageDispatcher;
        }

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var handlers = _resolver.ResolveAll<IEventHandlerAsync<TEvent>>();

            foreach (var handler in handlers)
                await handler.HandleAsync(@event);

            if (@event is IBusMessage message)
                await _busMessageDispatcher.DispatchAsync(message);
        }
    }
}
