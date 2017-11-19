using System;

namespace Weapsy.Mediator.Domain
{
    public class DomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
