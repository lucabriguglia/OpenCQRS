using OpenCqrs.Abstractions.Bus;

namespace OpenCqrs.Bus.Rabbitmq.Factories
{
    public interface IMessageFactory
    {
        byte[] CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage;
    }
}
