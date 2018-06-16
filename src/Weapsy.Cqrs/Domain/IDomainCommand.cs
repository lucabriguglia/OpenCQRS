using System;
using Weapsy.Cqrs.Commands;

namespace Weapsy.Cqrs.Domain
{
    public interface IDomainCommand : ICommand
    {
        Guid Id { get; set; }
        Guid AggregateRootId { get; set; }
        Guid UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
