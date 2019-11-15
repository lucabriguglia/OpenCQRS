namespace Kledex.Bus
{
    public abstract class BusTopicMessage : BusMessage, IBusTopicMessage
    {
        public string TopicName { get; set; }
    }
}
