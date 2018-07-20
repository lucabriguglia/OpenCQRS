using System;
using OpenCqrs.Bus;
using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Events
{
    public class ProductCreatedBusMessage : DomainEvent, IBusQueueMessage
    {
        public string Title { get; set; }
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "product-created";
    }
}
