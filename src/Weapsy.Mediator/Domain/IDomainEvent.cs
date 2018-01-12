using System;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateRootId { get; set; }
        int Version { get; set; }
        new DateTime TimeStamp { get; set; }
        Guid UserId { get; set; }
        string Source { get; set; }
    }
}
