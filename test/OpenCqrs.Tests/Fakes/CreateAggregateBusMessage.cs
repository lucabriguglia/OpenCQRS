using System;
using OpenCqrs.Abstractions.Bus;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Bus;

namespace OpenCqrs.Tests.Fakes
{
    public class CreateAggregateBusMessage : DomainCommand, IBusQueueMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }
        public string QueueName { get; set; } = "create-something";
    }
}
