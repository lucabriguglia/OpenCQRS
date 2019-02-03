using System;

namespace OpenCqrs.Abstractions.Domain
{
    public abstract class DomainCommand : IDomainCommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AggregateRootId { get; set; }
        public string UserId { get; set; }
        public string Source { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public int? ExpectedVersion { get; set; }
    }
}
