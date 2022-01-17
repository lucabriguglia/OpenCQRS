using System;
using Kledex.Bus;
using Kledex.Events;
using System.Collections.Generic;

namespace Kledex.Tests.Fakes
{
    public class SomethingCreated : Event, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "queue-name";
        public IDictionary<string, object> Properties { get; set; }
    }
}
