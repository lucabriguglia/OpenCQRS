using System;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateId { get; set; }
        int Version { get; set; }
        new DateTime TimeStamp { get; set; }
    }
}
