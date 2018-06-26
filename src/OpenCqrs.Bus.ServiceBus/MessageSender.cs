using System;
using System.Threading.Tasks;

namespace OpenCqrs.Bus.ServiceBus
{
    public class MessageSender : IMessageSender
    {
        public Task SendAsync<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            throw new NotImplementedException();
        }
    }
}
