using System;
using System.Threading.Tasks;
using Kledex.Bus.RabbitMQ.Factories;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

// ReSharper disable StringLiteralTypo

namespace Kledex.Bus.RabbitMQ.Topics
{
    public class TopicClient : ITopicClient
    {
        private readonly IMessageFactory _messageFactory;
        private readonly string _connectionString;

        public TopicClient(IMessageFactory messageFactory, IOptions<ServiceBusConfiguration> serviceBusConfiguration)
        {
            _messageFactory = messageFactory;
            _connectionString = serviceBusConfiguration.Value.ConnectionString;
        }

        /// <inheritdoc />
        public Task SendAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage
        {
            if (string.IsNullOrEmpty(message.TopicName))
                throw new ApplicationException("Topic name is mandatory");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString)
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: message.TopicName, type: "fanout");

                var body = _messageFactory.CreateMessage(message);

                channel.BasicPublish(exchange: message.TopicName, routingKey: string.Empty, basicProperties: null, body: body);
            }

            return Task.CompletedTask;
        }
    }
}