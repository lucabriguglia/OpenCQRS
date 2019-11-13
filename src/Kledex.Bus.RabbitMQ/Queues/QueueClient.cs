using System;
using System.Threading.Tasks;
using Kledex.Bus.RabbitMQ.Factories;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Kledex.Bus.RabbitMQ.Queues
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
        public Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage
        {
            if (string.IsNullOrEmpty(message.QueueName))
            {
                throw new ApplicationException("Queue name is mandatory");
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString)
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: message.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = _messageFactory.CreateMessage(message);

                channel.BasicPublish(exchange: string.Empty, routingKey: message.QueueName, basicProperties: null, body: body);
            }

            return Task.CompletedTask;
        }
    }
}