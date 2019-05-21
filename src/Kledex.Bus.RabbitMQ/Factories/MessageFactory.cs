using System.Text;
using Newtonsoft.Json;

namespace OpenCqrs.Bus.RabbitMQ.Factories
{
    public class MessageFactory : IMessageFactory
    {
        /// <inheritdoc />
        public byte[] CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            var json = JsonConvert.SerializeObject(message);
            var result = Encoding.UTF8.GetBytes(json);
            return result;
        }
    }
}