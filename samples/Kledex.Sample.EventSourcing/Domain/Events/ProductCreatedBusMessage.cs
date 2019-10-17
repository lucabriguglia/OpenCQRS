using System;
using Kledex.Bus;
using Kledex.Domain;
using System.Collections.Generic;

namespace Kledex.Samples.EventSourcing.Domain.Events
{
    public class ProductCreatedBusMessage : DomainEvent, IBusTopicMessage
    {
        public string Title { get; set; }
        public ProductStatus Status { get; set; }

        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string TopicName { get; set; } = "product-created";
        public IDictionary<string, object> Properties { get; set; }
    }
}
