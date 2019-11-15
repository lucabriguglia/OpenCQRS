using System;
using Kledex.Events;

namespace Kledex.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid Id { get; set; }
        Guid AggregateRootId { get; set; }
        Guid CommandId { get; set; }
        void Update(IDomainCommand command);
    }
}
