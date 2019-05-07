using System;
using System.Collections.Generic;
using OpenCqrs.Bus;
using OpenCqrs.Domain;

namespace OpenCqrs.Tests.Fakes
{
    public class AggregateCreated : DomainEvent, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "queue-name";
        public string SessionId { get; set; }
        public string CorrelationId { get; set; }
        public IDictionary<string, object> UserProperties { get; set; }
        public string Label { get; set; }
    }
}
