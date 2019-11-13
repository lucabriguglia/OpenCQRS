using Kledex.Bus.RabbitMQ.Factories;
using Kledex.Bus.RabbitMQ.Queues;
using Kledex.Bus.RabbitMQ.Topics;
using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable InconsistentNaming

namespace Kledex.Bus.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IKledexServiceBuilder AddRabbitMQProvider(this IKledexServiceBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IQueueClient, QueueClient>()
                .AddTransient<ITopicClient, TopicClient>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
