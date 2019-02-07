using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Bus.Configuration;
using OpenCqrs.Bus.Queues;
using OpenCqrs.Bus.Rabbitmq.Factories;
using OpenCqrs.Bus.Rabbitmq.Queues;
using OpenCqrs.Bus.Rabbitmq.Topics;
using OpenCqrs.Bus.Topics;
using OpenCqrs.Extensions;

namespace OpenCqrs.Bus.Rabbitmq.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IOpenCqrsServiceBuilder AddRabbitmqProvider(this IOpenCqrsServiceBuilder builder,
            IConfiguration configuration)
        {
            builder.Services
                .Configure<ServiceBusConfiguration>(configuration.GetSection(Constants.ServiceBusConfigurationSection));

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IQueueClient, QueueClient>()
                .AddTransient<ITopicClient, TopicClient>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
            
    }
}
