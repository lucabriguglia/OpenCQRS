using System;
using Kledex.Bus;
using Kledex.Domain;

namespace Kledex.Tests.Fakes
{
    public class CreateAggregateBusMessage : DomainCommand, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "create-something";
    }
}
