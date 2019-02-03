using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Bus.Configuration;
using OpenCqrs.Bus.Queues;
using OpenCqrs.Bus.ServiceBus.Factories;
using OpenCqrs.Bus.ServiceBus.Queues;
using OpenCqrs.Bus.ServiceBus.Topics;
using OpenCqrs.Bus.Topics;
using OpenCqrs.Extensions;

namespace OpenCqrs.Bus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IOpenCqrsServiceBuilder AddServiceBusProvider(this IOpenCqrsServiceBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<ServiceBusConfiguration>(configuration.GetSection(Constants.ServiceBusConfigurationSection));

            builder.Services.AddTransient<IBusMessageDispatcher, BusMessageDispatcher>();
            builder.Services.AddTransient<IQueueClient, QueueClient>();
            builder.Services.AddTransient<ITopicClient, TopicClient>();
            builder.Services.AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
