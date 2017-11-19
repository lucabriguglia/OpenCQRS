using System;
using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Tests.Fakes
{
    public class CreateAggregate : IDomainCommand
    {
        public Guid AggregateId { get; set; }
    }
}
