using System.Text;
using Newtonsoft.Json;

namespace OpenCqrs.Bus.Rabbitmq.Factories
{
    public class MessageFactory : IMessageFactory
    {
        public byte[] CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            var json = JsonConvert.SerializeObject(message);
            var serviceBusMessage = Encoding.UTF8.GetBytes(json);

            return serviceBusMessage;
        }
    }
}