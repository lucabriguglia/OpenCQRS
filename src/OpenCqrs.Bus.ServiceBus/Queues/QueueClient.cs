using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using OpenCqrs.Bus.ServiceBus.Configuration;
using OpenCqrs.Bus.ServiceBus.Factories;

namespace OpenCqrs.Bus.ServiceBus.Queues
{
    public class QueueClient : IQueueClient
    {
        private readonly IMessageFactory _messageFactory;
        private readonly string _connectionString;

        public QueueClient(IMessageFactory messageFactory, IOptions<ServiceBusConfiguration> serviceBusConfiguration)
        {
            _messageFactory = messageFactory;
            _connectionString = serviceBusConfiguration.Value.ConnectionString;
        }

        public async Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage
        {
            var client = new Microsoft.Azure.ServiceBus.QueueClient(new ServiceBusConnectionStringBuilder(_connectionString)
            {
                EntityPath = message.QueueName
            });

            var serviceBusMessage = _messageFactory.CreateMessage(message);

            await client.SendAsync(serviceBusMessage);

            await client.CloseAsync();
        }
    }
}