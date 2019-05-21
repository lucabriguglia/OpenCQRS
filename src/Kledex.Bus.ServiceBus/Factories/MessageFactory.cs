using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace OpenCqrs.Bus.ServiceBus.Factories
{
    public class MessageFactory : IMessageFactory
    {
        /// <inheritdoc />
        public Message CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            var json = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(json));

            if (message.ScheduledEnqueueTimeUtc.HasValue)
                serviceBusMessage.ScheduledEnqueueTimeUtc = message.ScheduledEnqueueTimeUtc.Value;

            return serviceBusMessage;
        }
    }
}