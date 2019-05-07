using System;
using System.Collections.Generic;

namespace Kledex.Bus
{
    public interface IBusMessage
    {
        DateTime? ScheduledEnqueueTimeUtc { get; set; }
        string SessionId { get; set; }
        string CorrelationId { get; set; }
        IDictionary<string, object> UserProperties { get; set; }
        string Label { get; set; }
    }
}
