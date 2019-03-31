using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Bus.RabbitMQ.Factories;
using OpenCqrs.Bus.RabbitMQ.Queues;
using OpenCqrs.Bus.RabbitMQ.Topics;
using OpenCqrs.Extensions;

// ReSharper disable InconsistentNaming

namespace OpenCqrs.Bus.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IOpenCqrsServiceBuilder AddRabbitMQProvider(this IOpenCqrsServiceBuilder builder, IConfiguration configuration)
        {
            builder.Services
                .Configure<ServiceBusConfiguration>(configuration.GetSection("RabbitMQConfiguration"));

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IQueueClient, QueueClient>()
                .AddTransient<ITopicClient, TopicClient>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
