using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenCqrs.Domain
{
    public interface IAggregateRootWithEvents : IAggregateRoot
    {
        ReadOnlyCollection<IDomainEvent> Events { get; }
        void ApplyEvents(IEnumerable<IDomainEvent> events);
    }
}
