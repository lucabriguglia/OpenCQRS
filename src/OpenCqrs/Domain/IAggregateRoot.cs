using System;

namespace OpenCqrs.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
