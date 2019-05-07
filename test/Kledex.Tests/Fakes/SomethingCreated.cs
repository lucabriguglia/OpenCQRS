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
        public string SessionId {get; set; }
        public string CorrelationId {get; set; }
        public IDictionary<string, object> UserProperties {get; set; }
        public string Label {get; set; }
    }
}
