using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace OpenCqrs.Bus.ServiceBus.Factories
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

            if (message.Properties != null)
            {
                foreach (var prop in message.Properties)
                {
                    // We could use reflexion here, but i believe we should bet on performace and simplicity.
                    // If not, then we can consider adding more of this properties
                    // Last note, we should do the same for rabbit.
                    if (prop.Key == "Label")
                        serviceBusMessage.Label = message.Properties[prop.Key] as string;
                    else if (prop.Key == "SessionId")
                        serviceBusMessage.SessionId = message.Properties[prop.Key] as string;
                    else if (prop.Key == "CorrelationId")
                        serviceBusMessage.CorrelationId = message.Properties[prop.Key] as string;
                    else if (prop.Key == "ScheduledEnqueueTimeUtc" && message.Properties[prop.Key] is System.DateTime ScheduledEnqueueTimeUtc)
                        serviceBusMessage.ScheduledEnqueueTimeUtc = ScheduledEnqueueTimeUtc;
                    else
                        serviceBusMessage.UserProperties.Add(prop.Key, prop.Value);
                }
            }

            serviceBusMessage.UserProperties.Add(new KeyValuePair<string, object>(AssemblyQualifiedNamePropertyName, message.GetType().AssemblyQualifiedName));

            return serviceBusMessage;
        }
    }
}