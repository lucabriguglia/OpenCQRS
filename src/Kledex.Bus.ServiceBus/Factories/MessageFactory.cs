using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Kledex.Bus.ServiceBus.Factories
{
    public class MessageFactory : IMessageFactory
    {
        public static readonly string AssemblyQualifiedNamePropertyName = "AssemblyQualifiedName";
        /// <inheritdoc />
        public Message CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            var json = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(json))
            {
                ContentType = "application/json"
            };

            if (message.ScheduledEnqueueTimeUtc.HasValue)
                serviceBusMessage.ScheduledEnqueueTimeUtc = message.ScheduledEnqueueTimeUtc.Value;
            if (string.IsNullOrWhiteSpace(message.Label))
                serviceBusMessage.Label = message.Label;
            if (!string.IsNullOrWhiteSpace(message.SessionId))
                serviceBusMessage.SessionId = message.SessionId;
            if (!string.IsNullOrWhiteSpace(message.CorrelationId))
                serviceBusMessage.CorrelationId = message.CorrelationId;

            serviceBusMessage.UserProperties.Add(new KeyValuePair<string, object>(AssemblyQualifiedNamePropertyName, message.GetType().AssemblyQualifiedName));

            return serviceBusMessage;
        }
    }
}