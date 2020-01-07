using System;
using Kledex.Commands;

namespace Kledex.Domain
{
    public interface IDomainCommand : ICommand
    {
        Guid AggregateRootId { get; set; }
        int? ExpectedVersion { get; set; }
        bool? SaveCommandData { get; set; }
    }

    public interface IDomainCommand<out TAggregateRoot> : IDomainCommand 
        where TAggregateRoot : IAggregateRoot
    {
    }
}
