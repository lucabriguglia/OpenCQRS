using System;
using Kledex.Bus;
using Kledex.Domain;
using System.Collections.Generic;

namespace Kledex.Examples.Domain.Events
{
    public class ProductCreatedBusMessage : DomainEvent, IBusTopicMessage
    {
        public string Title { get; set; }
        public ProductStatus Status { get; set; }

        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string TopicName { get; set; } = "product-created";
        public string SessionId { get; set; }
        public string CorrelationId { get; set; }
        public IDictionary<string, object> UserProperties { get; set; }
        public string Label { get; set; }
    }
}
