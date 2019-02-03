using System;

namespace OpenCqrs.Abstractions.Bus
{
    public interface IBusMessage
    {
        DateTime? ScheduledEnqueueTimeUtc { get; set; }
    }
}
