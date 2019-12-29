using Kledex.Bus.RabbitMQ.Factories;
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
        public static IKledexServiceBuilder AddRabbitMQ(this IKledexServiceBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IBusProvider, BusProvider>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
