using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Bus;
using Kledex.Dependencies;

namespace Kledex.Events
{
    /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                await PublishAsync(@event);
            }
        }
    }
}
