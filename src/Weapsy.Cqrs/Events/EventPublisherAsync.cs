using System;
using System.Threading.Tasks;
using Weapsy.Cqrs.Dependencies;

namespace Weapsy.Cqrs.Events
{
    /// <inheritdoc />
    /// <summary>
    /// EventPublisherAsync
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.Events.IEventPublisherAsync" />
    public class EventPublisherAsync : IEventPublisherAsync
    {
        private readonly IResolver _resolver;

        public EventPublisherAsync(IResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventHandlers = _resolver.ResolveAll<IEventHandlerAsync<TEvent>>();

            foreach (var handler in eventHandlers)
                await handler.HandleAsync(@event);
        }
    }
}
