using System;
using System.Collections.Generic;

namespace Kledex.Bus
{
    public abstract class BusMessage : IBusMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public IDictionary<string, object> Properties { get; set; }
    }
}
