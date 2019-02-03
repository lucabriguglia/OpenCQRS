using Microsoft.Azure.ServiceBus;
using OpenCqrs.Abstractions.Bus;

namespace OpenCqrs.Bus.ServiceBus.Factories
{
    public interface IMessageFactory
    {
        Message CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage;
    }
}
