using System;
using Kledex.Commands;

namespace Kledex.Domain
{
    public abstract class DomainCommand<TAggregateRoot> : Command, IDomainCommand<TAggregateRoot> 
        where TAggregateRoot : IAggregateRoot
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AggregateRootId { get; set; } = Guid.NewGuid();
        public int? ExpectedVersion { get; set; }
        public bool? SaveCommandData { get; set; }
    }
}
