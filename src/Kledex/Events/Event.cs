using System;

namespace Kledex.Events
{
    public class Event : IEvent
    {
        public string UserId { get; set; }
        public string Source { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
