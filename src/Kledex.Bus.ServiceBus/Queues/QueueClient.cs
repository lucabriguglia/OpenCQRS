using System;
using System.Threading.Tasks;
using Kledex.Bus.ServiceBus.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;

namespace Kledex.Bus.ServiceBus.Queues
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

        /// <inheritdoc />
        public async Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage
        {
            if (string.IsNullOrEmpty(message.QueueName))
                throw new ApplicationException("Queue name is mandatory");

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