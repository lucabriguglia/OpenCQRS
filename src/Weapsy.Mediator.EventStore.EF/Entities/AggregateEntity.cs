using System;
using System.Collections.Generic;

namespace Weapsy.Mediator.EventStore.EF.Entities
{
    public class AggregateEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();
    }
}
