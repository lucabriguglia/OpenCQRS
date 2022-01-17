using Kledex.Bus.RabbitMQ.Factories;
using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable InconsistentNaming

namespace Kledex.Bus.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddRabbitMQ(this IKledexServiceBuilder builder)
        {
            return AddRabbitMQ(builder, opt => { });
        }

        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IKledexServiceBuilder AddRabbitMQ(this IKledexServiceBuilder builder, Action<BusOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.Services.Configure(configureOptions);

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IBusProvider, RabbitMQBusProvider>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
