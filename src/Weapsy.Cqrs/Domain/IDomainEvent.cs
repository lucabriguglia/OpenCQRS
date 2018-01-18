using System;
using Weapsy.Cqrs.Events;

namespace Weapsy.Cqrs.Domain
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
