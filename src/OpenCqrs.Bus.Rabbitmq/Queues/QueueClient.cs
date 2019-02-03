using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenCqrs.Bus.Rabbitmq.Configuration;
using OpenCqrs.Bus.Rabbitmq.Factories;
using RabbitMQ.Client;

namespace OpenCqrs.Bus.Rabbitmq.Queues
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
            if (string.IsNullOrEmpty(message.QueueName))
                throw new ApplicationException("Queue name is mandatory");

            var factory = new ConnectionFactory { Uri  = new Uri(_connectionString) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = _messageFactory.CreateMessage(message);

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: message.QueueName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}