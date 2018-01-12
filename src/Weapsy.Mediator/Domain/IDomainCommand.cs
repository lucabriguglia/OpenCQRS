using System;
using Weapsy.Mediator.Commands;

namespace Weapsy.Mediator.Domain
{
    public interface IDomainCommand : ICommand
    {
        Guid AggregateRootId { get; set; }
        Guid UserId { get; set; }
        string Source { get; set; }
    }
}
