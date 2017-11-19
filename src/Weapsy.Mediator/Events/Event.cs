using System;

namespace Weapsy.Mediator.Events
{
    public class Event : IEvent
    {
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
