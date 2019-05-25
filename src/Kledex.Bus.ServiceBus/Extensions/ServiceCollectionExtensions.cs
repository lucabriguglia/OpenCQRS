using Kledex.Bus.ServiceBus.Factories;
using Kledex.Bus.ServiceBus.Queues;
using Kledex.Bus.ServiceBus.Topics;
using Kledex.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Bus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IKledexServiceBuilder AddServiceBusProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
        {
            builder.Services
                .Configure<ServiceBusConfiguration>(configuration.GetSection("ServiceBusConfiguration"));

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IQueueClient, QueueClient>()
                .AddTransient<ITopicClient, TopicClient>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
