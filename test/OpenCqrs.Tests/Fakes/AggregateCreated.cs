using System;
using OpenCqrs.Abstractions.Bus;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Bus;

namespace OpenCqrs.Tests.Fakes
{
    public class AggregateCreated : DomainEvent, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "queue-name";
    }
}
