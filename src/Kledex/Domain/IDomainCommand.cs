using System;
using Kledex.Commands;

namespace Kledex.Domain
{
    public interface IDomainCommand : ICommand
    {
        Guid Id { get; set; }
        Guid AggregateRootId { get; set; }
        string UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
        int? ExpectedVersion { get; set; }
        bool? SaveCommandData { get; set; }
        bool? UseTransaction { get; set; }
    }

    public interface IDomainCommand<out TAggregateRoot> : IDomainCommand 
        where TAggregateRoot : IAggregateRoot
    {
    }
}
