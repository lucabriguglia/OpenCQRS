using System;

namespace Kledex.Bus
{
    public interface IBusMessage
    {
        DateTime? ScheduledEnqueueTimeUtc { get; set; }
    }
}
