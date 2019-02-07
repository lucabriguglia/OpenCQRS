using System;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Options;
using OpenCqrs.Abstractions.Bus;
using OpenCqrs.Bus.Configuration;
using OpenCqrs.Bus.Queues;
using OpenCqrs.Bus.Rabbitmq.Factories;
using OpenCqrs.Bus.Rabbitmq.Shared;
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

            var channel = new ConnectionFactory { Uri = new Uri(_connectionString) }
                .CreateConnection()
                .CreateModel();

            RabbitmqResourceManager
                .EnlistToTransaction(channel, Transaction.Current);

            var body = _messageFactory.CreateMessage(message);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: message.QueueName,
                basicProperties: null,
                body: body);
        }
    }
}