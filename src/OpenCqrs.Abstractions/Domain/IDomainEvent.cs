using System;
using OpenCqrs.Abstractions.Events;

namespace OpenCqrs.Abstractions.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid Id { get; set; }
        Guid AggregateRootId { get; set; }
        Guid CommandId { get; set; }
        string UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
        void Update(IDomainCommand command);
    }
}
