using System;

namespace Weapsy.Cqrs.Bus
{
    public interface IBusMessage
    {
        string QueueName { get; set; }
        DateTime? EnqueueDateTime { get; set; }
    }
}
