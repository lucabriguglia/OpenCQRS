using System;
using Weapsy.Mediator.Dependencies;

namespace Weapsy.Mediator.Events
{
    /// <inheritdoc />
    /// <summary>
    /// EventPublisher
    /// </summary>
    /// <seealso cref="T:Weapsy.Mediator.Events.IEventPublisher" />
    public class EventPublisher : IEventPublisher
    {
        private readonly IResolver _resolver;

        public EventPublisher(IResolver resolver)
        {
            _resolver = resolver;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventHandlers = _resolver.ResolveAll<IEventHandler<TEvent>>();

            foreach (var handler in eventHandlers)
                handler.Handle(@event);
        }
    }
}
