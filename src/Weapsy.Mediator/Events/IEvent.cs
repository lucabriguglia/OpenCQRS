using System;

namespace Weapsy.Mediator.Events
{
    public interface IEvent
    {
        DateTime TimeStamp { get; set; }
    }
}
