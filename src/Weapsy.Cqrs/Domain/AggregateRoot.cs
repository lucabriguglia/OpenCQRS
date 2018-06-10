using System;
using System.Collections.Generic;
using ReflectionMagic;

namespace Weapsy.Cqrs.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public Guid Id { get; protected set; }
        public IList<IDomainEvent> Events { get; } = new List<IDomainEvent>();

        protected AggregateRoot()
        {
            Id = Guid.Empty;
        }

        protected AggregateRoot(Guid id)
        {
            if (id == Guid.Empty)
                id = Guid.NewGuid();

            Id = id;
        }

        public void ApplyEvents(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
                this.AsDynamic().Apply(@event);
        }

        /// <summary>
        /// Adds the event to the new events collection and calls the related apply method.
        /// </summary>
        /// <param name="event">The event.</param>
        protected void AddEvent(IDomainEvent @event)
        {
            Events.Add(@event);
            this.AsDynamic().Apply(@event);
        }
    }
}
