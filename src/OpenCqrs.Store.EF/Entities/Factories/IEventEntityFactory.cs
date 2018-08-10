using OpenCqrs.Domain;
using System;

namespace OpenCqrs.Store.EF.Entities.Factories
{
    public interface IEventEntityFactory
    {
        EventEntity CreateEvent(IDomainEvent @event, int version);
    }
}
