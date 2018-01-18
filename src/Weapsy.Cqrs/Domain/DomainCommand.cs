using System;

namespace Weapsy.Cqrs.Domain
{
    public class DomainCommand : IDomainCommand
    {
        public Guid AggregateRootId { get; set; }
        public Guid UserId { get; set; }
        public string Source { get; set; }
    }
}
