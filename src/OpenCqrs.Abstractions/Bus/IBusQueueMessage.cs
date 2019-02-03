namespace OpenCqrs.Abstractions.Bus
{
    public interface IBusQueueMessage : IBusMessage
    {
        string QueueName { get; set; }
    }
}
