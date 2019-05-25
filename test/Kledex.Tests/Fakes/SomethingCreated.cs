using System;
using Kledex.Bus;
using Kledex.Events;

namespace Kledex.Tests.Fakes
{
    public class SomethingCreated : Event, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "queue-name";
    }
}
