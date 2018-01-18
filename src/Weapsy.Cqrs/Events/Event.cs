using System;

namespace Weapsy.Cqrs.Events
{
    public class Event : IEvent
    {
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
