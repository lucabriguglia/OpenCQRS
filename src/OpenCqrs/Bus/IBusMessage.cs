using System;

namespace OpenCqrs.Bus
{
    public interface IBusMessage
    {
        string QueueName { get; set; }
        DateTime? EnqueueDateTime { get; set; }
    }
}
