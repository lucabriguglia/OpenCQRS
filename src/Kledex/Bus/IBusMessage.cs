using System;
using System.Collections.Generic;

namespace Kledex.Bus
{
    public interface IBusMessage
    {
        [Obsolete("Set add a new entry in Properties with the key ScheduledEnqueueTimeUtc instead")]
        DateTime? ScheduledEnqueueTimeUtc { get; set; }
        IDictionary<string, object> Properties { get; set; }
    }
}
