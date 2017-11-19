using System;

namespace Weapsy.Mediator.Domain
{
    public class DomainCommand : IDomainCommand
    {
        public Guid AggregateId { get; set; }
    }
}
