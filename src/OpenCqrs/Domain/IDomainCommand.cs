using System;
using OpenCqrs.Commands;

namespace OpenCqrs.Domain
{
    public interface IDomainCommand : ICommand
    {
        Guid Id { get; set; }
        Guid AggregateRootId { get; set; }
        string UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
        int? ExpectedVersion { get; set; }
    }
}
