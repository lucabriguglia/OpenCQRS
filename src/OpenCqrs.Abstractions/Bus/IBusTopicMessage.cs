namespace OpenCqrs.Abstractions.Bus
{
    public interface IBusTopicMessage : IBusMessage
    {
        string TopicName { get; set; }
    }
}
