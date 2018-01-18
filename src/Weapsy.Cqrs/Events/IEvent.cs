using System;

namespace Weapsy.Cqrs.Events
{
    public interface IEvent
    {
        DateTime TimeStamp { get; set; }
    }
}
