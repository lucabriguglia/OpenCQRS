using System;

namespace OpenCqrs.Bus
{
    public interface IBusMessage
    {
        DateTime? ScheduledEnqueueTimeUtc { get; set; }
    }
}
