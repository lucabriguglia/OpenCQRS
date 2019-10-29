using System;
using Kledex.Events;

namespace Kledex.Domain
{
    public abstract class DomainEvent : Event, IDomainEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AggregateRootId { get; set; }
        public Guid CommandId { get; private set; }
        public string UserId { get; private set; }
        public string Source { get; private set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public void Update(IDomainCommand command)
        {
            CommandId = command.Id;
            UserId = command.UserId;
            Source = command.Source;
        }
    }
}
