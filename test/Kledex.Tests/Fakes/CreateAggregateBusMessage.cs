using System;
using Kledex.Bus;
using Kledex.Domain;
using System.Collections.Generic;

namespace Kledex.Tests.Fakes
{
    public class CreateAggregateBusMessage : DomainCommand, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "create-something";
        public string SessionId { get; set; }
        public string CorrelationId { get; set; }
        public IDictionary<string, object> UserProperties { get; set; }
        public string Label { get; set; }
    }
}
