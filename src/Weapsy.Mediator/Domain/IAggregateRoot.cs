using System;
using System.Collections.Generic;

namespace Weapsy.Mediator.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        IList<IDomainEvent> Events { get; }
        void ApplyEvents(IEnumerable<IDomainEvent> events);
    }
}
