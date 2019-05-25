using Kledex.Bus.RabbitMQ.Factories;
using Kledex.Bus.RabbitMQ.Queues;
using Kledex.Bus.RabbitMQ.Topics;
using Kledex.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace Kledex.Bus.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IKledexServiceBuilder AddRabbitMQProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
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
