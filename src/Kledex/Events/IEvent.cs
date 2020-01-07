using System;

namespace Kledex.Events
{
    public interface IEvent
    {
        Guid Id { get; set; }
        string UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
