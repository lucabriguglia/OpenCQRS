using System;
using Kledex.Bus;
using Kledex.Domain;
using System.Collections.Generic;

namespace Kledex.Tests.Fakes
{
    public class CreateAggregateBusMessage : DomainCommand<Aggregate>, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "create-something";
        public IDictionary<string, object> Properties { get; set; }
    }
}
