using System;
using System.Threading.Tasks;
using Kledex.Bus.ServiceBus.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Kledex.Bus.ServiceBus.Queues
{
    public class QueueClient : IQueueClient
    {
        private readonly IMessageFactory _messageFactory;
        private readonly string _connectionString;

        public QueueClient(IMessageFactory messageFactory, IConfiguration configuration)
        {
            _messageFactory = messageFactory;
            _connectionString = configuration.GetConnectionString("KledexMessageBus");
        }

        /// <inheritdoc />
        public async Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage
        {
            if (string.IsNullOrEmpty(message.QueueName))
            {
                throw new ApplicationException("Queue name is mandatory");
            }

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