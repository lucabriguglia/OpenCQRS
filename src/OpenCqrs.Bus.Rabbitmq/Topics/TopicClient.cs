using System;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Options;
using OpenCqrs.Abstractions.Bus;
using OpenCqrs.Bus.Configuration;
using OpenCqrs.Bus.Rabbitmq.Factories;
using OpenCqrs.Bus.Rabbitmq.Shared;
using OpenCqrs.Bus.Topics;
using RabbitMQ.Client;

namespace OpenCqrs.Bus.Rabbitmq.Topics
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

        public async Task SendAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage
        {
            if (string.IsNullOrEmpty(message.TopicName))
                throw new ApplicationException("Topic name is mandatory");

            var channel = new ConnectionFactory { Uri = new Uri(_connectionString) }
                .CreateConnection()
                .CreateModel();

            RabbitmqResourceManager
                .EnlistToTransaction(channel, Transaction.Current);

            var body = _messageFactory.CreateMessage(message);

            channel.BasicPublish(
                exchange: message.TopicName,
                routingKey: string.Empty,
                basicProperties: null,
                body: body);
        }
    }
}