using System;
using System.Threading.Tasks;
using Kledex.Bus.ServiceBus.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Kledex.Bus.ServiceBus.Topics
{
    public class TopicClient : ITopicClient
    {
        private readonly IMessageFactory _messageFactory;
        private readonly string _connectionString;

        public TopicClient(IMessageFactory messageFactory, IConfiguration configuration)
        {
            _messageFactory = messageFactory;
            _connectionString = configuration.GetConnectionString("KledexMessageBus");
        }

        /// <inheritdoc />
        public async Task SendAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage
        {
            if (string.IsNullOrEmpty(message.TopicName))
                throw new ApplicationException("Topic name is mandatory");

            var client = new Microsoft.Azure.ServiceBus.TopicClient(new ServiceBusConnectionStringBuilder(_connectionString)
            {
                EntityPath = message.TopicName
            });

            var serviceBusMessage = _messageFactory.CreateMessage(message);

            await client.SendAsync(serviceBusMessage);

            await client.CloseAsync();
        }
    }
}