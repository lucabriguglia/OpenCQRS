using System;

namespace Weapsy.Mediator.EventStore.EF.Entities
{
    public class EventEntity
    {
        public Guid AggregateId { get; set; }
        public int SequenceNumber { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid UserId { get; set; }
        public string Source { get; set; }

        public virtual AggregateEntity Aggregate { get; set; }
    }
}
