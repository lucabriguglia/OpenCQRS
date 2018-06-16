using System;
using Weapsy.Cqrs.Dependencies;

namespace Weapsy.Cqrs.Events
{
    /// <inheritdoc />
    /// <summary>
    /// EventPublisher
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.Events.IEventPublisher" />
    public class EventPublisher : IEventPublisher
    {
        private readonly IResolver _resolver;

        public EventPublisher(IResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var handlers = _resolver.ResolveAll<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
                handler.Handle(@event);
        }
    }
}
