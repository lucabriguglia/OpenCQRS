using System;

namespace Weapsy.Mediator.EventStore.EF.Entities
{
    public class EventEntity
    {
        public Guid AggregateId { get; set; }
        public int SequenceNumber { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual AggregateEntity Aggregate { get; set; }
    }
}
