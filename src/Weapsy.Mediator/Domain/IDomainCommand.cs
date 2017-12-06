using System;
using Weapsy.Mediator.Commands;

namespace Weapsy.Mediator.Domain
{
    public interface IDomainCommand : ICommand
    {
        Guid AggregateRootId { get; set; }
    }
}
