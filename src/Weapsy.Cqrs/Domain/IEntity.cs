using System;

namespace Weapsy.Cqrs.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
